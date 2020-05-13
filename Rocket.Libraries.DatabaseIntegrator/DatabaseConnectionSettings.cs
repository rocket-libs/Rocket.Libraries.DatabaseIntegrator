using System;
using System.Linq;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public class DatabaseConnectionSettings
    {
        public string ConnectionString { get; set; }

        internal static bool IsDevelopment => IsInSet ("development");

        internal static bool IsStaging => IsInSet ("staging");

        internal static bool IsProduction => IsInSet ("production");

        private static bool IsInSet (params string[] candidates)
        {
            var aspnetCoreEnv = Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT");
            return candidates.ToList ().Any (a => a.Equals (aspnetCoreEnv, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}