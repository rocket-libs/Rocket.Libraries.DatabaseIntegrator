using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider, TDatabaseIntegrationEventHandlers> (this IServiceCollection services)
        where TConnectionProvider : class, IConnectionProvider
        where TDatabaseIntegrationEventHandlers : class, IDatabaseIntegrationEventHandlers<TIdentifier>
        {

            services
            .AddScoped<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>> ()
            .AddTransient<IConnectionProvider, TConnectionProvider> ()
            .AddTransient<IDatabaseIntegrationEventHandlers<TIdentifier>, TDatabaseIntegrationEventHandlers> ();
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}