using Microsoft.AspNetCore.Http;

namespace Blp.NetCoreLearning.WebApp.Services
{
    public static class StaticHttpContextAccessor
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => _httpContextAccessor.HttpContext;
    }
}
