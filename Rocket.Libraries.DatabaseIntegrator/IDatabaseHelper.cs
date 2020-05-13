namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Rocket.Libraries.Qurious;

    public interface IDatabaseHelper<TId> : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        Task<int> ExecuteAsync(string sql, object param = null);

        Task<ImmutableList<TModel>> GetManyAsync<TModel>(QBuilder qbuilder)
            where TModel : ModelBase<TId>;

        Task<TModel> GetSingleAsync<TModel>(string query);

        Task<TModel> GetSingleAsync<TModel>(QBuilder qbuilder)
            where TModel : ModelBase<TId>;

        void RollBackTransaction();

        Task SaveAsync<TModel>(TModel model)
            where TModel : ModelBase<TId>;
    }
}