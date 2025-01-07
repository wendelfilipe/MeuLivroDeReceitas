using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.Migrations
{
    public class DataBaseMigration
    {
        public static void Migrate(string connectiongString, IServiceProvider serviceProvider)
        {
            EnsureDatabaseCreated(connectiongString);

            MigrationDataBase(serviceProvider);
        }

        private static void EnsureDatabaseCreated(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

            var dataBaseName = connectionStringBuilder.Database;

            connectionStringBuilder.Remove("DataBase");

            using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("name", dataBaseName);

            var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

            if(!records.Any() )
            {
                dbConnection.Execute($"CREATE DATABASE {dataBaseName}");
            }
        }

        private static void MigrationDataBase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();

            runner.MigrateUp();
        }
    }
}
