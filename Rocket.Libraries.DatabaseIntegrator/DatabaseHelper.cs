﻿namespace Rocket.Libraries.DatabaseIntegrator
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Dapper.Contrib.Extensions;
    using Dapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Rocket.Libraries.DatabaseIntegrator.Tests;
    using Rocket.Libraries.Qurious;
    using Rocket.Libraries.Sessions.SessionProvision;
    using Rocket.Libraries.Validation.Services;

    public class DatabaseHelper<TId> : IDatabaseHelper<TId>
    {
        private readonly ILogger<DatabaseHelper<TId>> logger;

        private readonly ISessionProvider<long> sessionProvider;
        private readonly IConnectionOpener connectionOpener;
        private readonly IEventHandlers<TId> eventHandlers;
        private readonly Action<string> fnLogSelects;

        private IDbTransaction _transaction;

        private IDbConnection _connection;

        private bool _isDisposed = false;

        private DatabaseConnectionSettings databaseConnectionSettings;

        public DatabaseHelper (
            IOptions<DatabaseConnectionSettings> DatabaseConnectionSettingsOptions,
            ILogger<DatabaseHelper<TId>> logger,
            ISessionProvider<long> sessionProvider,
            IConnectionOpener connectionOpener,
            IEventHandlers<TId> eventHandlers)
        {
            this.logger = logger;
            this.sessionProvider = sessionProvider;
            this.connectionOpener = connectionOpener;
            this.eventHandlers = eventHandlers;
            this.databaseConnectionSettings = DatabaseConnectionSettingsOptions.Value;
            if (DatabaseConnectionSettings.IsDevelopment)
            {
                fnLogSelects = (query) => logger.LogTrace ($"{Environment.NewLine}------{Environment.NewLine}{query}{Environment.NewLine}-------{Environment.NewLine}");
            }
            else
            {
                fnLogSelects = (query) => { };
            }
        }

        private bool InTransaction => _transaction != default (IDbTransaction);

        private IDbConnection Connection
        {
            get
            {
                if (_connection == null && !_isDisposed)
                {
                    DataValidator.ThrowIfRuleFailed (string.IsNullOrEmpty (databaseConnectionSettings.ConnectionString), $"Connection string not yet set.");
                    _connection = connectionOpener.Open (databaseConnectionSettings.ConnectionString);
                    _connection.Open ();
                }

                return _connection;
            }
        }

        [ExcludeFromCoverage]
        public async Task<int> ExecuteAsync (string sql, object param = null)
        {
            return await Connection.ExecuteAsync (sql, param, _transaction);
        }

        [ExcludeFromCoverage]
        public async Task<ImmutableList<TModel>> GetManyAsync<TModel> (QBuilder qbuilder)
        where TModel : ModelBase<TId>
        {
            ApplyOnBeforeSelectActions (qbuilder);
            var query = qbuilder.Build ();
            return await GetManyAsync<TModel> (query);
        }

        [ExcludeFromCoverage]
        public async Task<TModel> GetSingleAsync<TModel> (string query)
        {
            var result = await GetManyAsync<TModel> (query);
            DataValidator.ThrowIfRuleFailed (result.Count > 1, $"Expected only one result for query. Instead got '{result.Count}'{Environment.NewLine}Query Was: {Environment.NewLine}{Environment.NewLine}{query}");
            return result.FirstOrDefault ();
        }

        [ExcludeFromCoverage]
        public async Task<TModel> GetSingleAsync<TModel> (QBuilder qbuilder)
        where TModel : ModelBase<TId>

            {
                ApplyOnBeforeSelectActions (qbuilder);
                var query = qbuilder.Build ();
                return await GetSingleAsync<TModel> (query);
            }

        public async Task SaveAsync<TModel> (TModel model)
        where TModel : ModelBase<TId>
        {
            if (model.IsNew)
            {
                if (eventHandlers.BeforeCreate != null)
                {
                    eventHandlers.BeforeCreate (model);
                }
                model.Created = DateTime.Now;
                await Connection.InsertAsync (model, transaction : _transaction);
            }
            else
            {
                if (eventHandlers.BeforeUpdate != null)
                {
                    eventHandlers.BeforeUpdate (model);
                }
                await Connection.UpdateAsync (model, transaction : _transaction);
            }
        }

        [ExcludeFromCoverage]
        public void BeginTransaction ()
        {
            if (!_isDisposed)
            {
                DataValidator.ThrowIfRuleFailed (InTransaction, "A database transaction is already in progress. Cannot nest transactions");
                _transaction = Connection.BeginTransaction ();
            }
        }

        [ExcludeFromCoverage]
        public void CommitTransaction ()
        {
            if (InTransaction && !_isDisposed)
            {
                _transaction.Commit ();
                ExitTransaction ();
            }
        }

        [ExcludeFromCoverage]
        public void RollBackTransaction ()
        {
            if (InTransaction && !_isDisposed)
            {
                _transaction.Rollback ();
                ExitTransaction ();
            }
        }

        [ExcludeFromCoverage]
        private async Task<ImmutableList<TModel>> GetManyAsync<TModel> (string query)
        {
            fnLogSelects (query);
            var result = await Connection.QueryAsync<TModel> (query, transaction : _transaction);
            return result.ToImmutableList ();
        }

        private void ApplyOnBeforeSelectActions (QBuilder qBuilder)
        {
            if (eventHandlers.BeforeSelect != null)
            {
                eventHandlers.BeforeSelect (qBuilder);
            }
        }

        private void ExitTransaction ()
        {
            _transaction?.Dispose ();
            _transaction = null;
        }

        #region IDisposable Support

#pragma warning disable SA1201 // Elements should appear in the correct order

        private bool disposedValue = false; // To detect redundant calls
#pragma warning restore SA1201 // Elements should appear in the correct order

        public void Dispose (bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ExitTransaction ();
                    _connection?.Close ();
                    _connection?.Dispose ();
                    _connection = null;
                    _isDisposed = true;
                }

                disposedValue = true;
            }
        }

        // ~DatabaseHelper()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
#pragma warning disable SA1202 // Elements should be ordered by access

        public void Dispose ()
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose (true);

            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}