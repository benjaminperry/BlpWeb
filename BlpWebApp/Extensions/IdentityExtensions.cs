using BlpData;
using BlpWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlpWebApp.Extensions
{
    public static class IdentityExtensions
    {
        public static IdentityBuilder SetupIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityBuilder identityBuilder = services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BlpContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.TryAddTransient<IEmailSender, EmailSender>();

            // > The IUserClaimsPrincipalFactory added by AddIdentityCore does not support roles, so we need to replace
            //   it.
            ServiceDescriptor sd = new ServiceDescriptor(
                typeof(IUserClaimsPrincipalFactory<IdentityUser>),
                typeof(UserClaimsPrincipalFactory<IdentityUser, IdentityRole>),
                ServiceLifetime.Scoped);
            services.Replace(sd);

            return identityBuilder;
        }
    }
}
