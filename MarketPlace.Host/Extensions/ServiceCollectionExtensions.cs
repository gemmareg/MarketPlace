using RabbitMQ.Client;

namespace MarketPlace.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration);
            services.AddHostedService<UserRegisteredConsumer>();

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
