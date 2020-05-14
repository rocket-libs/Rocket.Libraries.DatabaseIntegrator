using System;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator.Tests
{
    public interface IDatabaseIntegrationEventHandlers<TIdentifier>
    {
        Action<QBuilder,Type> BeforeSelect { get; }

        Action<ModelBase<TIdentifier>> BeforeCreate { get; }

        Action<ModelBase<TIdentifier>> BeforeUpdate { get; }
    }
}