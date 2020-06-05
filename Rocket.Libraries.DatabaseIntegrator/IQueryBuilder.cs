using System;
using System.Threading.Tasks;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IQueryBuilder<TIdentifier>
    {
        Func<string> Build { get; set; }
        
        void PrepareSelectByIdQuery<TModel> (TIdentifier id, bool? showDeleted);

        void PrepareSelectAll<TModel> (int? page, ushort? pageSize, bool? showDeleted);
    }
}