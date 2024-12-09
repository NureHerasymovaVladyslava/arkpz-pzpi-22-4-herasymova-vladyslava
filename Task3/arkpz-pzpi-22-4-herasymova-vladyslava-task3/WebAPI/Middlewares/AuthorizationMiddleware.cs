using Core.Models;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Managers;

namespace WebAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
        public string[] Roles { get; }

        public AuthorizeAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserRoleManager userRoleManager)
        {
            var endpoint = context.GetEndpoint();

            var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (authorizeAttribute != null)
            {
                var user = context.Items["User"] as AppUser;

                if (user == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Please log in.");
                    return;
                }

                if (authorizeAttribute.Roles.Length > 0)
                {
                    if (!(await userRoleManager.IsUserInRoles(user, authorizeAttribute.Roles)))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden: You do not have the required role.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
