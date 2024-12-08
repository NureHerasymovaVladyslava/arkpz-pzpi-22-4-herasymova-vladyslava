
using Core.Configurations;
using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebAPI.Hubs;
using WebAPI.Managers;
using WebAPI.Middlewares;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(SensorLogRepository));
            builder.Services.AddScoped(typeof(SensorRepository));
            builder.Services.AddScoped(typeof(LockRepository));
            builder.Services.AddScoped(typeof(LockLogRepository));
            builder.Services.AddScoped(typeof(AppUserRepository));
            builder.Services.AddScoped(typeof(ControlRepository));
            builder.Services.AddScoped(typeof(DocumentLogRepository));
            builder.Services.AddScoped(typeof(DatabaseAdminManager));

            builder.Services.AddScoped(typeof(UserRoleManager));
            builder.Services.AddScoped(typeof(EmailManager));

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseRouting();

            app.UseSession();

            app.UseMiddleware<SessionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.MapHub<AlertHub>("/alertHub");

            app.Run();
        }
    }
}
