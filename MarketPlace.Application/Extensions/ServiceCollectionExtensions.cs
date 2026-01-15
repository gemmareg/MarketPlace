using FluentValidation;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MarketPlace.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IOrderService, OrderService>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
