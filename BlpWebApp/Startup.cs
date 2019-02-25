using BlpData;
using BlpWebApp.Extensions;
using BlpWebApp.Filters;
using BlpWebApp.Models;
using BlpWebApp.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using System.Net;

namespace BlpWebApp
{
    public class Startup
    {
        public IHostingEnvironment Environment;
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Environment = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration.GetSection("Authentication"));
            services.Configure<EmailOptions>(Configuration.GetSection("SMTP"));

            services.AddDistributedSqlServerCache(opt =>
                {
                    opt.ConnectionString = Configuration.GetConnectionString("blpweb");
                    opt.SchemaName = "dbo";
                    opt.TableName = "SQLCache";
                });

            services.AddMvc(opts =>
                {
                    opts.Filters.Add(typeof(AdalTokenAcquisitionExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices();

            services.SetupDataProtection(Configuration);

            services.SetupAuthenticationAndIdentity2(Configuration);
            
            Container container = new Container();

            container.Configure(config => 
                {
                    config.AddRegistry(new ServicesRegistry(Configuration, Environment));
                    config.Populate(services);
                });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
                {
                    Console.WriteLine("*** *** *** THIS IS A MIDDLEWARE TEST 1 *** *** ***");
                    await next.Invoke();
                    Console.WriteLine("*** *** *** THIS IS A MIDDLEWARE TEST 2 *** *** ***");
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ForwardedHeadersOptions fho = new ForwardedHeadersOptions()
                { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto };

            fho.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:10.0.0.0"), 120));

            app.UseForwardedHeaders(fho);

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(configureRoutes =>
                {
                    configureRoutes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
