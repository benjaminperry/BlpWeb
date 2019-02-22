using BlpData;
using BlpWebApp.Models;
using BlpWebApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StructureMap;
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
            For<IEmailSender>().Use<EmailSender>().Singleton();
        }

        private static BlpWebBaseContext CreateBlpWebContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<BlpWebBaseContext>();
            builder.UseSqlServer(connectionString);
            return new BlpWebContext(builder.Options);
        }
    }
}
