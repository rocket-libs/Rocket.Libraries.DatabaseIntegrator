using System;
using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        internal static string DatabaseType;

        /// <summary>
        /// Initialize database integrator
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="registerSingletons">Flag to determine whether services should be registered as singletons. Setting this to false registers them 'Scoped'</param>
        /// <param name="databaseType">Specify type of database. Useful for when you're using 'MiniProfiler.Integrations' with a database other than SQL Server</param>
        /// <typeparam name="TIdentifier">Data type of your model's identifier.</typeparam>
        /// <typeparam name="TConnectionProvider">Class that supplies the database connection.</typeparam>
        /// <returns></returns>
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