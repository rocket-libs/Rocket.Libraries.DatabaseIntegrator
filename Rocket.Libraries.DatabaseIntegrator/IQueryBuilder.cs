using System;
using System.Threading.Tasks;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface ISelectHelper<TModel, TIdentifier>
    {
        Func<string> Build { get; set; }

        Func<TIdentifier, bool?, Task> GetSingleByIdAsync { get; set; }

        Func<int?, ushort?, bool?, Task> GetPagedAsync { get; set; }
    }
}