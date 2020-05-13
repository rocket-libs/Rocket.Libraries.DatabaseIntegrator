using System.Data;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IConnectionOpener
    {
        IDbConnection Open (string connectionString);
    }
}