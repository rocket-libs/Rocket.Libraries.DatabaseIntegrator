using System;
using System.Threading.Tasks;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IQueryBuilder<TIdentifier>
    {
        string Build();

        void ApplyPaging<TModel>(TypedPropertyNamedResolver<TModel> field, uint page, ushort pageSize);

        void ManageDeletedRecordsVisibility<TModel>(bool showDeleted);

        void PrepareSelectByIdQuery<TModel>(TIdentifier id, bool? showDeleted);

        void PrepareSelectAll<TModel>(int? page, ushort? pageSize, bool? showDeleted);
    }
}