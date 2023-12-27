namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Rocket.Libraries.DatabaseIntegrator.Tests;

    /// <summary>
    /// Represents a helper interface for interacting with a database.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the identifier used in the database.</typeparam>
    public interface IDatabaseHelper<TIdentifier>
    {
        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the current database transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Executes a SQL query asynchronously and returns the number of affected rows.
        /// </summary>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="param">The parameters to be used in the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the number of affected rows.</returns>
        Task<int> ExecuteAsync(string sql, object param = null);

        /// <summary>
        /// Retrieves multiple models from the database asynchronously.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to retrieve.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to be used in the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an immutable list of the retrieved models.</returns>
        [ExcludeFromCoverage]
        Task<ImmutableList<TModel>> GetManyAsync<TModel>(string query, Dictionary<string, object> parameters = null);

        /// <summary>
        /// Retrieves a single model from the database asynchronously.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to retrieve.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to be used in the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the retrieved model.</returns>
        Task<TModel> GetSingleAsync<TModel>(string query, Dictionary<string, object> parameters = null)
            where TModel : ModelBase<TIdentifier>;

        /// <summary>
        /// Rolls back the current database transaction.
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// Saves a model to the database asynchronously.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to save.</typeparam>
        /// <param name="model">The model to save.</param>
        /// <param name="isUpdate">A flag indicating whether the model is being updated or inserted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveAsync<TModel>(TModel model, bool isUpdate)
            where TModel : ModelBase<TIdentifier>;

        /// <summary>
        /// Executes a SQL query asynchronously and returns the scalar result.
        /// </summary>
        /// <typeparam name="T">The type of the scalar result.</typeparam>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="param">The parameters to be used in the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the scalar result.</returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null);
    }
}