using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IReaderBase<TModel,TId> : IDisposable
        where TModel : ModelBase<TId>
    {
        Task<ImmutableList<TModel>> GetAsync(int? page, ushort? pageSize, bool? showDeleted);
        Task<TModel> GetByIdAsync(TId id, bool? showDeleted);
    }
}