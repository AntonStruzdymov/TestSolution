using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using TestDB;
using TestTask.Abstractions;
using TestTask.Abstractions.Repositiories;
using TestTask.Abstractions.Services;
using TestTask.Implementation;
using TestTask.Implementation.Repositories;
using TestTask.Implementation.Services;

namespace TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services.AddDbContext<Context>(
                options =>
                {
                    var connString = builder.Configuration
                        .GetConnectionString("DefaultConnection");
                    options.UseSqlServer(connString, b=>b.MigrationsAssembly("TestTask"));
                });

            builder.Services.AddControllers().AddNewtonsoftJson(opt=>
            opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IJWTRepositiory, JWTRepository>();

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IJWTService, JWTService>();
            

            builder.Services.AddAutoMapper(typeof(Program));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TestTask.xml");
                options.IncludeXmlComments(xmlPath);
            });
            
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