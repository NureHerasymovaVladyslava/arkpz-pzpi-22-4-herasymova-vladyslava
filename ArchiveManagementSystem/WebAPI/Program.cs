
using DAL;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
