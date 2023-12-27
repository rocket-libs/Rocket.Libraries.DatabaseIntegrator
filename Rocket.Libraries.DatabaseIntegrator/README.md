# Rocket.Libraries.DatabaseIntegrator

This library is a layer on top of Dapper ORM and makes its goal is making working with your database more structured and even easier.

Think of it like a framework for Dapper ORM, this way every one on your team writes code in a predictable and hence easy to understand manner.

It has extensively been tested with MS SQL and MySQL/MariaDB and even used in multiple production environments.

## Installing
The library is available on nuget and can be installed using
```bash
    dotnet add package Rocket.Libraries.DatabaseIntegrator
```

## Usage
This library is meant to use dotnet's default dependancy injection.


### Configuration
1. Let's tell the library how to connect to your database

In your appsettings.json, add the following:

```json
{
     //.... Your other settings

    "DatabaseConnectionSettings": {
        "ConnectionString": "Server=localhost;Database=database_name;Uid=sam;Pwd=the-password;charset=utf8;convertzerodatetime=true;"
  }
}
```

2. Read the connection string during runtime.

Create a class to read the connection string and return a connection to the database.

```csharp

using System.Data;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Rocket.Libraries.DatabaseIntegrator;

namespace App.Database
{
    public class DatabaseConnectionProvider : IConnectionProvider
    {
        private readonly string connectionString;

        public DatabaseConnectionProvider(
            IOptions<DatabaseConnectionSettings> databaseOptions)
        {

            connectionString = databaseOptions.Value.ConnectionString;
        }

        public IDbConnection Get(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);

            return connection;
        }
    }
}


```

Please note the ```using Rocket.Libraries.DatabaseIntegrator;``` included in the class. It gives you access to the the interface **IConnectionProvider** and to the object
**DatabaseConnectionSettings** which maps onto the connection string settings we created in appsettings.json




3. Let's configure our app to read the settings.
In Program.cs at the top of the file add

```csharp
    using Rocket.Libraries.DatabaseIntegrator;
```
Then in your services configuration, add the following lines
```csharp
    builder.Services.Configure<DatabaseConnectionSettings>(builder.Configuration.GetSection("DatabaseConnectionSettings"));
    builder.Services.InitializeDatabaseIntegrator<long,DatabaseConnectionProvider>();
```

In ```builder.Services.InitializeDatabaseIntegrator<long,DatabaseConnectionProvider>();``` the first generic parameter is the type that your 
database primary keys are, while the second is the database connection provider class we created in the previous step.

