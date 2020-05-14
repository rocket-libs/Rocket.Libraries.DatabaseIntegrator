using Microsoft.Extensions.DependencyInjection;
using Rocket.Libraries.Sessions.RequestHeaders;
using Rocket.Libraries.Sessions.SessionProvision;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider, TDatabaseIntegrationEventHandlers> (this IServiceCollection services)
        where TConnectionProvider : class, IConnectionProvider
        where TDatabaseIntegrationEventHandlers : class, IDatabaseIntegrationEventHandlers<TIdentifier>
        {

            services
            .AddTransient<IConnectionProvider, TConnectionProvider> ()
            .AddTransient<IDatabaseIntegrationEventHandlers<TIdentifier>, TDatabaseIntegrationEventHandlers> ()
            .AddScoped<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>> ()
            .AddTransient<ISessionProvider<TIdentifier>, SessionProvider<TIdentifier>> ()
            .AddTransient<IRequestHeaderReader, RequestHeaderReader> ();
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}