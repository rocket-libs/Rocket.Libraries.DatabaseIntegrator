using System.Data;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IConnectionProvider
    {
        IDbConnection Get (string connectionString);
    }
}