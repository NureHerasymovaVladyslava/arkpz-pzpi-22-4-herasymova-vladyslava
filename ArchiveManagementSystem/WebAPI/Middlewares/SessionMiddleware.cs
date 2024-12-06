using DAL;

namespace WebAPI.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppUserRepository userRepository)
        {
            var userId = context.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = await userRepository.GetByIdAsync(userId.Value);
                if (user != null)
                {
                    context.Items["User"] = user;
                }
            }

            await _next(context);
        }
    }
}
