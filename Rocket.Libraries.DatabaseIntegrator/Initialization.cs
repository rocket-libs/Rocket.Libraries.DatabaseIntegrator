using System;
using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        internal static string DatabaseType;
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider> (
            this IServiceCollection services, 
            bool registerSingletons = false,
            string databaseType = "")
        where TConnectionProvider : class, IConnectionProvider
        {

            if (registerSingletons)
            {
                services
                    .AddSingleton<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>> ()
                    .AddSingleton<IConnectionProvider, TConnectionProvider> ();
            }
            else
            {
                services
                    .AddScoped<IDatabaseHelper<TIdentifier>, DatabaseHelper<TIdentifier>> ()
                    .AddScoped<IConnectionProvider, TConnectionProvider> ();
            }
            Initialization.DatabaseType = databaseType;
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}