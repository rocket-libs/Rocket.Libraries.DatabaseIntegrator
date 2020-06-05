using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider, TDatabaseIntegrationEventHandlers,TQueryBuilder> (this IServiceCollection services)
        where TConnectionProvider : class, IConnectionProvider
        where TDatabaseIntegrationEventHandlers : class, IDatabaseIntegrationEventHandlers<TIdentifier>
        where TQueryBuilder : class, ISelectHelper<ModelBase<TIdentifier>,TIdentifier>
        {

            services
            .AddScoped<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>>()
            .AddTransient<IConnectionProvider, TConnectionProvider>()
            .AddTransient<IDatabaseIntegrationEventHandlers<TIdentifier>, TDatabaseIntegrationEventHandlers>()
            .AddTransient<ISelectHelper<ModelBase<TIdentifier>, TIdentifier>, TQueryBuilder>();
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}