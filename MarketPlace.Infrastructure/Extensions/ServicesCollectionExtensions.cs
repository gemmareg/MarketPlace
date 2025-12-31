using MarketPlace.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Infrastructure.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MarketPlaceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"))
            );



            return services;
        }
    }
}
