namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    public interface IDatabaseHelper<TIdentifier> : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        Task<int> ExecuteAsync(string sql, object param = null);

        Task<ImmutableList<TModel>> GetManyAsync<TModel>(
            ISelectHelper<TModel, TIdentifier> queryProvider)
            where TModel : ModelBase<TIdentifier>;

        Task<TModel> GetSingleAsync<TModel>(
            ISelectHelper<TModel, TIdentifier> queryProvider)
            where TModel : ModelBase<TIdentifier>;

        void RollBackTransaction();

        Task SaveAsync<TModel>(TModel model)
            where TModel : ModelBase<TIdentifier>;
    }
}