using MarketPlace.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarketPlace.Infrastructure.IntegrationTest.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public MarketPlaceDbContext DbContext { get; }
        public DatabaseFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build(); 

            var services = new ServiceCollection(); 
            
            services.AddDbContext<MarketPlaceDbContext>(options => options.UseSqlServer(config.GetConnectionString("SqlServer"))); 
            var provider = services.BuildServiceProvider(); 
            
            DbContext = provider.GetRequiredService<MarketPlaceDbContext>(); 
            
            // Ensure database exists
            DbContext.Database.EnsureCreated();
        }
        public void Dispose()
        {
            DbContext.Database.EnsureDeleted(); 
            DbContext.Dispose();
        }
    }
}
