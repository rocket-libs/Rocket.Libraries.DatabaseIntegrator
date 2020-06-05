using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public abstract class ReaderBase<TModel, TIdentifier> : IReaderBase<TModel, TIdentifier>
        where TModel : ModelBase<TIdentifier>
        {

            private readonly IDatabaseHelper<TIdentifier> databaseHelper;
            private readonly ISelectHelper<TIdentifier> selectHelper;

            public ReaderBase (
                IDatabaseHelper<TIdentifier> databaseHelper,
                ISelectHelper<TIdentifier> selectHelper)
            {
                this.databaseHelper = databaseHelper;
                this.selectHelper = selectHelper;
            }

            public void Dispose ()
            {

            }

            public async Task<TModel> GetByIdAsync (TIdentifier id, bool? showDeleted)
            {
                var query = selectHelper.GetSingleByIdAsync (id, showDeleted);
                return await databaseHelper.GetSingleAsync<TModel> (query);
            }

            public virtual async Task<ImmutableList<TModel>> GetAsync (int? page, ushort? pageSize, bool? showDeleted)
            {
                var query = selectHelper.GetPagedAsync (page, pageSize, showDeleted);
                return await databaseHelper.GetManyAsync<TModel>(query);
            }
        }

}