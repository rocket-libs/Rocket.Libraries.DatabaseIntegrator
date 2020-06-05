using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface ISelectHelper<TIdentifier>
    {
        Func<TIdentifier, bool?, string> GetSingleByIdAsync { get; set; }

        Func<int?, ushort?, bool?, string> GetPagedAsync { get; set; }
    }
}