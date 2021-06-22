using System;
using Microsoft.Extensions.DependencyInjection;
namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Initialization
    {
        internal static Func<object> IdGenerator;
        public static IServiceCollection InitializeDatabaseIntegrator<TIdentifier, TConnectionProvider> (this IServiceCollection services, Func<TIdentifier> idGenerator, bool scopeSingleton = false)
        where TConnectionProvider : class, IConnectionProvider
        {

            if (scopeSingleton)
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
            Initialization.IdGenerator = () => idGenerator ();
            DapperConfigurations.InitializeDapper ();
            return services;
        }
    }
}