using System;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IDatabaseIntegrationEventHandlers<TIdentifier>
    {
        Action<ModelBase<TIdentifier>> BeforeCreate { get; }

        Action<ModelBase<TIdentifier>> BeforeUpdate { get; }

        Action<ModelBase<TIdentifier>> BeforeDelete { get; }
    }
}