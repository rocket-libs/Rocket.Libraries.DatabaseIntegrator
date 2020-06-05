using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface ISelectHelper<TModel, TIdentifier>
    {
        Func<string> Build { get; set; }

        Func<TIdentifier, bool?, Task<TModel>> GetSingleByIdAsync { get; set; }

        Func<int?, ushort?, bool?, Task<ImmutableList<TModel>>> GetPagedAsync { get; set; }
    }
}