using System;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public class DatabaseIntegratorException : Exception
    {
        public DatabaseIntegratorException(string message) : base(message)
        {
        }
    }
}