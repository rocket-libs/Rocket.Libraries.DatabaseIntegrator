using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IReaderBase<TModel,TIdentifier> : IDisposable
        where TModel : ModelBase<TIdentifier>
    {
        Task<ImmutableList<TModel>> GetAsync(int? page, ushort? pageSize, bool? showDeleted);
        Task<TModel> GetByIdAsync(TIdentifier id, bool? showDeleted);
    }
}