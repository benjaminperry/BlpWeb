using Blp.NetCoreLearning.Entities;
using Blp.NetCoreLearning.WebApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StructureMap;
using System;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.DataMigration
{
    public class DataMigrationServicesRegistry : Registry
    {
        public DataMigrationServicesRegistry()
        {
            Scan(_ =>
            {
                _.AssemblyContainingType<IBuildModel>();
                _.AddAllTypesOf<IBuildModel>();
            });

            For<IConfiguration>().Use(CreateConfiguration()).Singleton();
            For<DataMigrationContext>().Use(ctx => CreateBlpContext(ctx.GetInstance<IConfiguration>(), ctx.GetAllInstances<IBuildModel>())).Transient();
        }

        private static IConfiguration CreateConfiguration()
        {
            IConfigurationBuilder cb = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddUserSecrets<Startup>(true)
                .AddEnvironmentVariables("BlpWeb");

            return  cb.Build();
        }

        private static DataMigrationContext CreateBlpContext(IConfiguration configuration, IEnumerable<IBuildModel> modelBuilders)
        {
            var builder = new DbContextOptionsBuilder<DataMigrationContext>();
            builder.UseSqlServer(configuration.GetConnectionString("blpweb"));
            return new DataMigrationContext(builder.Options, modelBuilders);
        }
    }
}
