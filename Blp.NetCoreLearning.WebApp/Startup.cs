using Blp.NetCoreLearning.WebApp.Extensions;
using Blp.NetCoreLearning.WebApp.Filters;
using Blp.NetCoreLearning.WebApp.Options;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Net;

namespace Blp.NetCoreLearning.WebApp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Environment = env;
            Configuration = configuration;
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

            services
                .AddMvc(opts =>
                    {
                        opts.Filters.Add(typeof(AdalTokenAcquisitionExceptionFilter));
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
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

            app.UsePathBase(Configuration["PathBase"]);

            // > Exception Handling:
            app.UseGlobalExceptionHandler(exConfig =>
                {
                    exConfig.ContentType = "application/json";
                    exConfig.ResponseBody(ex => 
                        {
                            // ToDo: See if there is a way to access HttpContext in this handler.
                            if(Environment.IsDevelopment())
                            {
                                return JsonConvert.SerializeObject(new
                                    {
                                        //HttpContextAccessor.HttpContext.TraceIdentifier,
                                        Message = "Something went wrong.",
                                        ExceptionInfo = ex.ToString()
                                    });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new
                                    {
                                        //HttpContextAccessor.HttpContext.TraceIdentifier,
                                        Message = "Something went wrong."
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
            IPAddress address1 = IPAddress.Parse("::ffff:10.0.0.0");
            IPNetwork network1 = new IPNetwork(address1, 104);
            fho.KnownNetworks.Add(network1);
            app.UseForwardedHeaders(fho);

            // > Static files will be unauthenticated:
            app.UseStaticFiles();

            app.UseRouting();

            // > Anything below here will use authentication:
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });

            // > Pre endpoint routing way of using MVC:
            //app.UseMvc(configureRoutes =>
            //    {
            //        configureRoutes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //    });
        }
    }
}
