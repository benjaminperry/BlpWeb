using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Blp.NetCoreLearning.WebApp.Swagger
{
    public static class SwaggerAuthorizeMiddlewareApplicationBuilderExtension
    {
        public static IApplicationBuilder UseSwaggerAuthorize(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerAuthorizeMiddleware>();
        }
    }

    public class SwaggerAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments($"/{SwaggerDefaultValues.SwaggerRoute}") && !context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync();
                return;
            }

            await _next.Invoke(context);
        }
    }
}
