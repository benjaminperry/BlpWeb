﻿using Blp.NetCoreLearning.Data;
using Blp.NetCoreLearning.Entities;
using Blp.NetCoreLearning.WebApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StructureMap;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.WebApp
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Scan(_ =>
            {
                _.AssemblyContainingType<Startup>();
                _.AssemblyContainingType<IBuildModel>();
                _.WithDefaultConventions();
                _.AddAllTypesOf<IBuildModel>();
            });

            For<BlpContext>().Use(ctx => CreateBlpContext(configuration, ctx.GetAllInstances<IBuildModel>())).Transient();
            For<IEmailSender>().Use<EmailSender>().Singleton();
        }

        private static BlpContext CreateBlpContext(IConfiguration configuration, IEnumerable<IBuildModel> modelBuilders)
        {
            var builder = new DbContextOptionsBuilder<BlpContext>();
            builder.UseSqlServer(configuration.GetConnectionString("blpweb"));
            return new BlpContext(builder.Options, modelBuilders);
        }
    }
}
