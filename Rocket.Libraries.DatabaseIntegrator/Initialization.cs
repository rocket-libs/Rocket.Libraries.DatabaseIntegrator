using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider> (this IServiceCollection services)
        where TConnectionProvider : class, IConnectionProvider
        {

            services
            .AddScoped<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>> ()
            .AddTransient<IConnectionProvider, TConnectionProvider> ();
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}