using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Blp.NetCoreLearning.WebApp.Services
{
    public interface ITokenCacheFactory
    {
        TokenCache CreateForUser(ClaimsPrincipal user);
    }
}