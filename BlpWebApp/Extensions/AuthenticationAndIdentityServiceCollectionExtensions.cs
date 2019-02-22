using BlpData;
using BlpWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace BlpWebApp.Extensions
{
    public static class AuthenticationAndIdentityServiceCollectionExtensions
    {
        public const string AzureAdOpenIdConnectScheme = "AzureAdOpenIdConnect";

        public static IdentityBuilder SetupAuthenticationAndIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityBuilder identityBuilder = services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BlpWebBaseContext>()
                .AddDefaultTokenProviders();

            AuthenticationBuilder authenticationBuilder = services
                .AddAuthentication()
                .AddOpenIdConnect(
                    AzureAdOpenIdConnectScheme,
                    AzureAdOpenIdConnectScheme,
                    o =>
                    {
                        configuration.GetSection("Authentication").Bind(o);

                        o.Events = new OpenIdConnectEvents
                        {
                            OnAuthorizationCodeReceived = async ctx =>
                            {
                                HttpRequest request = ctx.HttpContext.Request;
                                //We need to also specify the redirect URL used
                                string currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);
                                //Credentials for app itself
                                var credential = new ClientCredential(ctx.Options.ClientId, ctx.Options.ClientSecret);

                                //Construct token cache
                                ITokenCacheFactory cacheFactory = ctx.HttpContext.RequestServices.GetRequiredService<ITokenCacheFactory>();
                                TokenCache cache = cacheFactory.CreateForUser(ctx.Principal);

                                var authContext = new AuthenticationContext(ctx.Options.Authority, cache);

                                //Get token for Microsoft Graph API using the authorization code
                                string resource = "https://graph.microsoft.com";
                                AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(
                                    ctx.ProtocolMessage.Code, new Uri(currentUri), credential, resource);

                                //Tell the OIDC middleware we got the tokens, it doesn't need to do anything
                                ctx.HandleCodeRedemption(result.AccessToken, result.IdToken);
                            }
                        };
                    });

            services.ConfigureApplicationCookie(
                o =>
                {
                    o.LoginPath = new PathString("/Identity/Account/Login");
                    o.LogoutPath = new PathString("/Identity/Account/Logout");
                    o.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                });

            return identityBuilder;
        }
    }
}
