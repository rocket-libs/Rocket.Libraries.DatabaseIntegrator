namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Rocket.Libraries.DatabaseIntegrator.Tests;

    public interface IDatabaseHelper<TIdentifier> : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        Task<int> ExecuteAsync(string sql, object param = null);
        [ExcludeFromCoverage]
        Task<ImmutableList<TModel>> GetManyAsync<TModel>(string query);
        Task<TModel> GetSingleAsync<TModel>(
            string query)
            where TModel : ModelBase<TIdentifier>;

        void RollBackTransaction();

        Task SaveAsync<TModel>(TModel model)
            where TModel : ModelBase<TIdentifier>;
    }
}