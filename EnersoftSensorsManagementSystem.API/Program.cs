
using EnersoftSensorsManagementSystem.API.Models;
using EnersoftSensorsManagementSystem.Application.Interfaces;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using EnersoftSensorsManagementSystem.Infrastructure.Repositories;
using EnersoftSensorsManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EnersoftSensorsManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // PostgreSQL
            builder.Services.AddDbContext<PostgresSensorDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            // MySQL
            builder.Services.AddDbContext<MySqlSensorDbContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection")!));

            // MSSQL
            builder.Services.AddDbContext<MssqlSensorDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlConnection")));

            // Caching service
            builder.Services.AddMemoryCache();

            // Versionning service 
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true; 
            });

            // Add services to the container.
            builder.Services.AddScoped<ISensorRepository, SensorRepository>();
            builder.Services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware for global exception handling
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception != null)
                    {
                        var errorResponse = new ErrorResponse
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An unexpected error occurred.",
                            Details = exception.Message
                        };

                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                });
            });

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
