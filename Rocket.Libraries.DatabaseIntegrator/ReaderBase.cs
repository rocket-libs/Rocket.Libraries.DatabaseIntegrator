using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public abstract class ReaderBase<TModel,TIdentifier> :  IReaderBase<TModel,TIdentifier>
        where TModel : ModelBase<TIdentifier>
    {
        
        private readonly IDatabaseHelper<TIdentifier> databaseHelper;

        public ReaderBase(
            IDatabaseHelper<TIdentifier> databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public void Dispose()
        {
            
        }

        public async Task<TModel> GetByIdAsync(TIdentifier id, bool? showDeleted)
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
                    .SetDeletedRecordsInclusionState<TModel,TIdentifier>(showDeleted);
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
                    .ApplyPaging<TModel, TIdentifier>(model => model.Id, page, pageSize)
                    .SetDeletedRecordsInclusionState<TModel,TIdentifier>(showDeleted);
                return await databaseHelper.GetManyAsync<TModel>(qbuilder);
            }
        }
    }
    
}