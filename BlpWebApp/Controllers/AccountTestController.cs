﻿using BlpWebApp.Extensions;
using BlpWebApp.Models;
using BlpWebApp.Options;
using BlpWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlpWebApp.Controllers
{
    [Authorize]
    public class AccountTestController : Controller
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly ITokenCacheFactory _tokenCacheFactory;
        private readonly AuthOptions _authOptions;

        public AccountTestController(ITokenCacheFactory tokenCacheFactory, IOptions<AuthOptions> authOptions)
        {
            _tokenCacheFactory = tokenCacheFactory;
            _authOptions = authOptions.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn(string redirectUrl = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignOut()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "/AccountTest/SignedOut"
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignedOut() => View();

        [HttpGet]
        public ActionResult UserClaims()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MsGraph()
        {
            HttpResponseMessage res = await QueryGraphAsync("/me");

            string rawResponse = await res.Content.ReadAsStringAsync();
            string prettyResponse =
                JsonConvert.SerializeObject(JsonConvert.DeserializeObject(rawResponse), Formatting.Indented);

            var model = new HomeMsGraphModel
            {
                GraphResponse = prettyResponse
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProfilePhoto()
        {
            HttpResponseMessage res = await QueryGraphAsync("/me/photo/$value");

            return File(await res.Content.ReadAsStreamAsync(), "image/jpeg");
        }

        //Normally this stuff would be in another service, not in the controller
        //But for the sake of an example, it is a bit easier
        private async Task<HttpResponseMessage> QueryGraphAsync(string relativeUrl)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/beta" + relativeUrl);

            string accessToken = await GetAccessTokenAsync();
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await Client.SendAsync(req);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            string authority = _authOptions.Authority;

            var cache = _tokenCacheFactory.CreateForUser(User);

            var authContext = new AuthenticationContext(authority, cache);

            //App's credentials may be needed if access tokens need to be refreshed with a refresh token
            string clientId = _authOptions.ClientId;
            string clientSecret = _authOptions.ClientSecret;
            var credential = new ClientCredential(clientId, clientSecret);
            var userId = User.GetObjectId();

            var result = await authContext.AcquireTokenSilentAsync(
                "https://graph.microsoft.com",
                credential,
                new UserIdentifier(userId, UserIdentifierType.UniqueId));

            return result.AccessToken;
        }
    }
}