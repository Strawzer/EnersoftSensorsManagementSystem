using EnersoftSensorsManagementSystem.API.Filters;
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
using Microsoft.OpenApi.Models;
using System.Text;

namespace EnersoftSensorsManagementSystem.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure PostgreSQL Database
        builder.Services.AddDbContext<PostgresSensorDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
        builder.Services.AddScoped<ISensorRepository, SensorRepository>(); // Repository for PostgreSQL
        builder.Services.AddScoped<ISensorTypeRepository, SensorTypeRepository>(); // Repository for PostgreSQL

        // Configure MSSQL Database
        builder.Services.AddDbContext<MssqlSensorDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlConnection")));
        builder.Services.AddScoped<IUserRepository, UserRepository>(); // Repository for MSSQL

        // Caching service
        builder.Services.AddMemoryCache();

        // Add JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "YourStrongKeyWithAtLeast32Characters!")
                    )
                };
            });

        // Add API Versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        // Add Swagger Configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Enersoft Sensors Management API V1", Version = "1.0" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "Enersoft Sensors Management API V2", Version = "2.0" });

            // Add JWT Bearer Security Definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' followed by your JWT token. Example: Bearer abc123"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Filter endpoints by API version
            c.DocInclusionPredicate((version, apiDesc) =>
            {
                var versions = apiDesc.ActionDescriptor.EndpointMetadata
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v.MajorVersion:0}" == version);
            });

            // Remove the version parameter from the Swagger UI
            c.OperationFilter<RemoveVersionFromParameter>();

            // Replace the version in the route with the exact version value
            c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
        });

        // Add Controllers
        builder.Services.AddControllers();

        // Add Services
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

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

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        var endpoints = app.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        foreach (var endpoint in endpoints)
        {
            Console.WriteLine(endpoint.DisplayName);
        }
        app.Run();

        
    }
}
