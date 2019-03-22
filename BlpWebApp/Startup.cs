using BlpWebApp.Extensions;
using BlpWebApp.Filters;
using BlpWebApp.Options;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Net;

namespace BlpWebApp
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Environment = env;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration.GetSection("Authentication"));
            services.Configure<EmailOptions>(Configuration.GetSection("SMTP"));
            
            services.AddHttpContextAccessor();

            services.AddDistributedSqlServerCache(opt =>
                {
                    opt.ConnectionString = Configuration.GetConnectionString("blpweb");
                    opt.SchemaName = "dbo";
                    opt.TableName = "SQLCache";
                });

            services.SetupDataProtection(Configuration);
            services.SetupAuthentication(Configuration);
            services.SetupIdentity(Configuration);

            services.AddMvc(opts =>
                {
                    opts.Filters.Add(typeof(AdalTokenAcquisitionExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices();

            Container container = new Container();

            container.Configure(config => 
                {
                    config.AddRegistry(new ServicesRegistry(Configuration, Environment));
                    config.Populate(services);
                });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // > Exception Handling:
            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // > Exception Handling:
            app.UseGlobalExceptionHandler(exConfig =>
            {
                exConfig.ContentType = "application/json";
                exConfig.ResponseBody(ex => 
                {
                    if(Environment.IsDevelopment())
                    {
                        return JsonConvert.SerializeObject(new
                            {
                                Message = "Something went wrong.",
                                HttpContextAccessor.HttpContext.TraceIdentifier,
                                ExceptionInfo = ex.ToString()
                            });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new
                            {
                                Message = "Something went wrong.",
                                HttpContextAccessor.HttpContext.TraceIdentifier,
                            });
                    }
                });
            });

            // > This is a simple sample of custom middleware:
            app.Use(async (context, next) =>
            {
                Console.WriteLine("*** *** *** THIS IS A MIDDLEWARE TEST 1 *** *** ***");
                await next.Invoke();
                Console.WriteLine("*** *** *** THIS IS A MIDDLEWARE TEST 2 *** *** ***");
            });

            // > Use forwarded headers so authentication will work when running under Kubernetes (KnownNetworks should
            //   probably be configured!):
            ForwardedHeadersOptions fho = new ForwardedHeadersOptions()
                { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto };
            fho.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:10.0.0.0"), 120));
            app.UseForwardedHeaders(fho);

            // > Static files will be unauthenticated:
            app.UseStaticFiles();

            // > Anything below here will use authentication:
            app.UseAuthentication();

            // > Use MVC:
            app.UseMvc(configureRoutes =>
                {
                    configureRoutes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
