using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BlpWebApi.Services
{
    public interface ITokenCacheFactory
    {
        TokenCache CreateForUser(ClaimsPrincipal user);
    }
}