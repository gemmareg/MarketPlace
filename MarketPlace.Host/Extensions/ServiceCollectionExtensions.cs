using MarketPlace.Host.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;

namespace MarketPlace.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
            services.AddSwaggerDocumentation();
            services.AddRabbitMq(configuration);
            services.AddHostedService<UserRegisteredConsumer>();

            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            return services;
        }

        private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MarketPlace API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token. Example: eyJhbGciOi..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionFactory>(_ =>
                new ConnectionFactory
                {
                    HostName = config["RabbitMQ:Host"],
                    Port = int.Parse(config["RabbitMQ:Port"] ?? "5672"),
                    UserName = config["RabbitMQ:User"],
                    Password = config["RabbitMQ:Password"]
                });

            services.AddSingleton<IConnection>(sp =>
                sp.GetRequiredService<IConnectionFactory>().CreateConnection());

            services.AddSingleton<IModel>(sp =>
                sp.GetRequiredService<IConnection>().CreateModel());

            return services;
        }
    }
}
