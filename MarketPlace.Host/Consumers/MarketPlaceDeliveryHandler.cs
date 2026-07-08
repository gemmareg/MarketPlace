using Auth.Contracts.Events;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared;
using MessageBroker.Consumer;
using System.Text.Json;

namespace MarketPlace.Host.Consumers
{
    /// <summary>
    /// Single entry point for every delivery MessageBrokerService sends to the "MarketPlace" consumer
    /// queue, regardless of event type — dispatches on <see cref="DeliveryMessage.EventType"/>.
    /// </summary>
    public class MarketPlaceDeliveryHandler(IUserService userService, ILogger<MarketPlaceDeliveryHandler> logger) : IDeliveryHandler
    {
        public async Task<DeliveryOutcome> HandleAsync(DeliveryMessage message, CancellationToken cancellationToken)
        {
            switch (message.EventType)
            {
                case "user.registered":
                    return await HandleUserRegistered(message);

                case "user.soft-deleted":
                    return await HandleUserSoftDeleted(message);

                default:
                    logger.LogWarning("Ignoring delivery {DeliveryId} of unknown event type {EventType}", message.DeliveryId, message.EventType);
                    return DeliveryOutcome.Success();
            }
        }

        private async Task<DeliveryOutcome> HandleUserRegistered(DeliveryMessage message)
        {
            var evt = JsonSerializer.Deserialize<UserRegisteredEvent>(message.Payload, JsonOptions.Default);
            if (evt is null)
                return DeliveryOutcome.Failure("Could not deserialize UserRegisteredEvent payload.");

            var result = await userService.CreateUserFromEvent(evt);
            if (!result.Success)
                logger.LogError("Failed creating user from event {DeliveryId}: {Message}", message.DeliveryId, result.Message);

            return result.Success ? DeliveryOutcome.Success() : DeliveryOutcome.Failure(result.Message);
        }

        private async Task<DeliveryOutcome> HandleUserSoftDeleted(DeliveryMessage message)
        {
            var evt = JsonSerializer.Deserialize<UserSoftDeletedEvent>(message.Payload, JsonOptions.Default);
            if (evt is null)
                return DeliveryOutcome.Failure("Could not deserialize UserSoftDeletedEvent payload.");

            var result = await userService.SoftDeleteUserFromEvent(evt);
            if (!result.Success)
                logger.LogError("Failed soft-deleting user from event {DeliveryId}: {Message}", message.DeliveryId, result.Message);

            return result.Success ? DeliveryOutcome.Success() : DeliveryOutcome.Failure(result.Message);
        }
    }
}
