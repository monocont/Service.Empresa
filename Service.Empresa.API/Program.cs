using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Empresa.Application.Commands.Empresa.CrearEmpresa;
using Service.Empresa.Application.Common.Behaviors;
using Service.Empresa.Application.Interfaces;
using Service.Empresa.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Service.Empresa.Infrastructure.Repositories;
using Service.Empresa.Infrastructure.Services;
using Service.Empresa.API.Middleware;

namespace Service.Empresa.API;

public class Program
{
    public static void Main(string[] args)
    {
var builder = WebApplication.CreateBuilder(args);

// Fix for Npgsql DateTime issues
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp",
                policy => policy.WithOrigins(allowedOrigins)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese el token JWT obtenido de Service.Seguridad"
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
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Service.Seguridad",
                    ValidAudience = "Monocont"
                };
            });

        builder.Services.AddAuthorization();

        var connectionString = builder.Configuration.GetConnectionString("EmpresaDb");

        builder.Services.AddDbContext<EmpresaDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        builder.Services.AddScoped<ICredencialSunatRepository, CredencialSunatRepository>();
        builder.Services.AddScoped<ITablaMaestraService, TablaMaestraService>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CrearEmpresaCommand).Assembly);
        });

        builder.Services.AddValidatorsFromAssembly(typeof(CrearEmpresaCommand).Assembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<ResponseFormattingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAngularApp");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}