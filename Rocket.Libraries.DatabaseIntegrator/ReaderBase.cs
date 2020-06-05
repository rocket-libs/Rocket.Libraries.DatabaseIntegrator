using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public abstract class ReaderBase<TModel,TIdentifier> :  IReaderBase<TModel,TIdentifier>
        where TModel : ModelBase<TIdentifier>
    {
        
        private readonly IDatabaseHelper<TIdentifier> databaseHelper;
        private readonly ISelectHelper<TModel, TIdentifier> queryBuilder;

        public ReaderBase(
            IDatabaseHelper<TIdentifier> databaseHelper,
            ISelectHelper<TModel, TIdentifier> queryBuilder)
        {
            this.databaseHelper = databaseHelper;
            this.queryBuilder = queryBuilder;
        }

        public void Dispose()
        {
            
        }

        public async Task<TModel> GetByIdAsync(TIdentifier id, bool? showDeleted)
        {
            queryBuilder.PrepareSelectByIdQuery<TModel>(id, showDeleted);
            return await databaseHelper.GetSingleAsync<TModel>(queryBuilder);
        }

        public virtual async Task<ImmutableList<TModel>> GetAsync(int? page, ushort? pageSize, bool? showDeleted)
        {
            queryBuilder.PrepareSelectAll<TModel>(page, pageSize, showDeleted);
            return await databaseHelper.GetManyAsync<TModel>(queryBuilder);
        }
    }
    
}