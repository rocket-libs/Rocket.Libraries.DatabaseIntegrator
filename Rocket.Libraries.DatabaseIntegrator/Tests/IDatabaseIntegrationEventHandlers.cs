using System;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator.Tests
{
    public interface IDatabaseIntegrationEventHandlers<TId>
    {
        Action<QBuilder> BeforeSelect { get; }

        Action<ModelBase<TId>> BeforeCreate { get; }

        Action<ModelBase<TId>> BeforeUpdate { get; }
    }
}