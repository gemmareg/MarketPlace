using Auth.Contracts.Events;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MarketPlace.Host.Consumers
{
    public class UserSoftDeletedConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly ILogger<UserSoftDeletedConsumer> _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        public UserSoftDeletedConsumer(IModel channel, ILogger<UserSoftDeletedConsumer> logger, IServiceScopeFactory scopeFactory)
        {
            _channel = channel;
            _logger = logger;
            _scopeFactory = scopeFactory;

            // Declare queue (Marketplace owns this)
            _channel.QueueDeclare(
                queue: "marketplace.user.softdeleted",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            // Bind queue to exchange
            _channel.QueueBind(
                queue: "marketplace.user.softdeleted",
                exchange: "auth.events",
                routingKey: "user.soft-deleted"
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var evt = JsonSerializer.Deserialize<UserSoftDeletedEvent>(json, JsonOptions.Default);

                Console.WriteLine($"[Marketplace] Received user.registered: {evt.UserId} - {evt.UserId}");

                using var scope = _scopeFactory.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var result = await userService.SoftDeleteUserFromEvent(evt);

                if (!result.Success)
                {
                    Console.WriteLine("[Marketplace] Failed to process event, requeueing...");
                    _logger.LogError($"Failed soft deleting user: {result.Message}");
                    // _channel.BasicNack(args.DeliveryTag, multiple: false, requeue: true); 
                    return;
                }

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(
                queue: "marketplace.user.registered",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }
}
