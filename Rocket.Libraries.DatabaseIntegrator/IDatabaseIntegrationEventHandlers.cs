using System;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IDatabaseIntegrationEventHandlers<TIdentifier>
    {
        Action<QBuilder,Type> BeforeSelect { get; }

        Action<ModelBase<TIdentifier>> BeforeCreate { get; }

        Action<ModelBase<TIdentifier>> BeforeUpdate { get; }

        Action<ModelBase<TIdentifier>> BeforeDelete { get; }
    }
}