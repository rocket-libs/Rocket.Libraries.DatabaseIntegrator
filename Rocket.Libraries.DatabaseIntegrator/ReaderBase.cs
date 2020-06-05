using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public abstract class ReaderBase<TModel, TIdentifier> : IReaderBase<TModel, TIdentifier>
        where TModel : ModelBase<TIdentifier>
        {

            private readonly IDatabaseHelper<TIdentifier> databaseHelper;
            private readonly ISelectHelper<TModel, TIdentifier> queryBuilder;

            public ReaderBase (
                IDatabaseHelper<TIdentifier> databaseHelper,
                ISelectHelper<TModel, TIdentifier> queryBuilder)
            {
                this.databaseHelper = databaseHelper;
                this.queryBuilder = queryBuilder;
            }

            public void Dispose ()
            {

            }

            public async Task<TModel> GetByIdAsync (TIdentifier id, bool? showDeleted)
            {
                return await queryBuilder.GetSingleByIdAsync (id, showDeleted);
            }

            public virtual async Task<ImmutableList<TModel>> GetAsync (int? page, ushort? pageSize, bool? showDeleted)
            {
                return await queryBuilder.GetPagedAsync (page, pageSize, showDeleted);
            }
        }

}