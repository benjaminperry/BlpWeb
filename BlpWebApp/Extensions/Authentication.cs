using BlpWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace BlpWebApp.Extensions
{
    public static class Authentication
    {
        public static AuthenticationBuilder SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddAuthentication(auth =>
                {
                    auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(opts =>
                {
                    configuration.GetSection("Authentication").Bind(opts);

                    opts.Events = new OpenIdConnectEvents
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
        }
    }
}
