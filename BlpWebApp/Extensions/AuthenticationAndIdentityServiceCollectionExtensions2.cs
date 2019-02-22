﻿using BlpData;
using BlpWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlpWebApp.Extensions
{
    public static class AuthenticationAndIdentityServiceCollectionExtensions2
    {
        public const string AzureAdOpenIdConnectScheme = "AzureAdOpenIdConnect";

        public static IdentityBuilder SetupAuthenticationAndIdentity2(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(
                    o =>
                    {
                        o.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                        o.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    })
                .AddCookie(
                    IdentityConstants.ApplicationScheme,
                    o =>
                    {
                        o.Events = new CookieAuthenticationEvents
                        {
                            OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                        };
                    })
                .AddCookie(
                    IdentityConstants.ExternalScheme, 
                    o =>
                    {
                        o.Cookie.Name = IdentityConstants.ExternalScheme;
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
            
            services.AddHttpContextAccessor();

            IdentityBuilder identityBuilder = services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BlpWebBaseContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.TryAddTransient<IEmailSender, EmailSender>();

            ServiceDescriptor sd = new ServiceDescriptor(
                typeof(IUserClaimsPrincipalFactory<IdentityUser>),
                typeof(UserClaimsPrincipalFactory<IdentityUser, IdentityRole>),
                ServiceLifetime.Scoped);

            services.Replace(sd);

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
