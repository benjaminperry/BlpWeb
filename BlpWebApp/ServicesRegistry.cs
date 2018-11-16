using BlpData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StructureMap;
using BlpWebApp.Services;
using BlpWebApp.Options;

namespace BlpWebApp
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry(IConfiguration configuration, IHostingEnvironment environment)
        {
            Scan(_ =>
            {
                _.AssemblyContainingType<Startup>();
                _.WithDefaultConventions();
            });
            
            For<BlpWebBaseContext>().Use(() => CreateBlpWebContext(configuration.GetConnectionString("blpweb"))).Transient();
        }

        private static BlpWebBaseContext CreateBlpWebContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<BlpWebBaseContext>();
            builder.UseSqlServer(connectionString);
            return new BlpWebContext(builder.Options);
        }
    }
}
