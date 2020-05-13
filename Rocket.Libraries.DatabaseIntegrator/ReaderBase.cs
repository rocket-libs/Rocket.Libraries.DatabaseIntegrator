using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public abstract class ReaderBase<TModel,TId> :  IReaderBase<TModel,TId>
        where TModel : ModelBase<TId>
    {
        
        private readonly IDatabaseHelper<TId> databaseHelper;

        public ReaderBase(
            IDatabaseHelper<TId> databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public void Dispose()
        {
            
        }

        public async Task<TModel> GetByIdAsync(TId id, bool? showDeleted)
        {
            using (var qbuilder = new QBuilder())
            {
                qbuilder
                    .UseSelector()
                    .Select<TModel>("*")
                    .Then()
                    .UseTableBoundFilter<TModel>()
                    .WhereEqualTo(model => model.Id, id)
                    .Then()
                    .SetDeletedRecordsInclusionState<TModel,TId>(showDeleted);
                return await databaseHelper.GetSingleAsync<TModel>(qbuilder);
            }
        }

        public virtual async Task<ImmutableList<TModel>> GetAsync(int? page, ushort? pageSize, bool? showDeleted)
        {
            using (var qbuilder = new QBuilder())
            {
                qbuilder
                    .UseSelector()
                    .Select<TModel>("*")
                    .Then()
                    .SetDeletedRecordsInclusionState<TModel,TId>(showDeleted);
                return await databaseHelper.GetManyAsync<TModel>(qbuilder);
            }
        }
    }
    
}