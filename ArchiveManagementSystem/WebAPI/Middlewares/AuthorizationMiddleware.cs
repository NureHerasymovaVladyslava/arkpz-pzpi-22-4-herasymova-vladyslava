using Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute { }

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var requiresAuthorization = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

            if (requiresAuthorization)
            {
                var user = context.Items["User"] as AppUser;

                if (user == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Please log in.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
