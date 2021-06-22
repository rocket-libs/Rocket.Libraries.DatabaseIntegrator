using System;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace Rocket.Libraries.DatabaseIntegrator
{
    internal static class DapperConfigurations
    {
        public static void InitializeDapper()
        {
            SqlMapperExtensions.TableNameMapper = GetTableName;
            if (!string.IsNullOrEmpty(Initialization.DatabaseType))
            {
                SqlMapperExtensions.GetDatabaseType = conn => Initialization.DatabaseType;
            }
        }

        private static string GetTableName(Type type)
        {
            var tableAttrib = type.GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), false);
            if (tableAttrib != null)
            {
                var firstTableAttrib = tableAttrib.SingleOrDefault();
                if (firstTableAttrib != null)
                {
                    return (firstTableAttrib as Dapper.Contrib.Extensions.TableAttribute).Name;
                }
            }

            return type.Name;
        }
    }
}