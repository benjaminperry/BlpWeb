using BlpData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DataMigration
{
    class DesignTimeBlpWebMigrationContextFactory : IDesignTimeDbContextFactory<BlpWebMigrationContext>
    {
        static private IConfigurationRoot _Configuration;
        public static IConfigurationRoot Configuration => _Configuration;

        public BlpWebMigrationContext CreateDbContext(string[] args)
        {
            ConfigInit();

            string connectionString = Configuration.GetConnectionString("blpweb");
            
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BlpWebBaseContext>();
            dbContextOptionsBuilder.UseSqlServer(connectionString);
            return new BlpWebMigrationContext(dbContextOptionsBuilder.Options);
        }

        static void ConfigInit()
        {
            var builder = new ConfigurationBuilder();

            builder
                .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables("BlpWeb");

            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env == "Development")
            {
                //builder.AddUserSecrets<Program>();
            }

            IConfigurationRoot configuration = builder.Build();
            _Configuration = configuration;
        }
    }
}
