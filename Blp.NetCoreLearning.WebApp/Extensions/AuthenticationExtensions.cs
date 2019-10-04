using Blp.NetCoreLearning.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Blp.NetCoreLearning.WebApp.Extensions
{
    public static class AuthenticationExtensions
    {
        public const string AzureAdOpenIdConnectScheme = "AzureAdOpenIdConnect";

        public static AuthenticationBuilder SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            AuthenticationBuilder authenticationBuilder =
            services
                .AddAuthentication(
                    o =>
                    {
                        o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    })
                .AddCookie(
                    IdentityConstants.ApplicationScheme,
                    o =>
                    {
                        o.LoginPath = new PathString("/Identity/Account/Login");
                        o.LogoutPath = new PathString("/Identity/Account/Logout");
                        o.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");

                        o.Events = new CookieAuthenticationEvents
                        {
                            // > Without this, any user who obtains a session cookie can keep using it even if they are
                            //   no longer a valid user in the user store. By default this validates every 30 minutes.
                            OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                        };
                    })
                .AddCookie(
                    IdentityConstants.ExternalScheme, 
                    o =>
                    {
                        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    })
                .AddCookie(
                    IdentityConstants.TwoFactorRememberMeScheme,
                    o =>
                    {
                        o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                        o.Events = new CookieAuthenticationEvents
                        {
                            OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                        };
                    })
                .AddCookie(
                    IdentityConstants.TwoFactorUserIdScheme,
                    o =>
                    {
                        o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    })
                .AddOpenIdConnect(
                    AzureAdOpenIdConnectScheme,
                    AzureAdOpenIdConnectScheme,
                    o =>
                    {
                        configuration.GetSection("Authentication").Bind(o);
                        o.TokenValidationParameters.ValidateIssuer = true;
                        o.TokenValidationParameters.ValidateAudience = true;
                        o.TokenValidationParameters.ValidateLifetime = true;
                        o.SignInScheme = IdentityConstants.ExternalScheme;

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
            
            return authenticationBuilder;
        }
    }
}
