using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BlpWebApp.Services
{
    public interface ITokenCacheFactory
    {
        TokenCache CreateForUser(ClaimsPrincipal user);
    }
}