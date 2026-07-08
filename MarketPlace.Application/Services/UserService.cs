using Auth.Contracts.Events;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.NonGeneric;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services
{
    public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<UserService> logger) : IUserService
    {
        public async Task<Result> CreateUserFromEvent(UserRegisteredEvent evt)
        {
            Guid userId;
            if (!Guid.TryParse(evt.userId, out userId))
            {
                logger.LogError("Failed to create user. {UserId} is not a valid guid", evt.userId);
                return Result.Fail("User id is not a valid guid");
            }

            var resultUser = User.Create(userId, evt.name);
            if (!resultUser.Success)
            {
                logger.LogError("Could not create user: {Message}", resultUser.Message);
                return Result.Fail($"Could not create user: {resultUser.Message}");
            }

            var user = resultUser.Data;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} created successfully from event", user.Id);
            return Result.Ok();
        }

        public async Task<Result> SoftDeleteUserFromEvent(UserSoftDeletedEvent evt)
        {
            Guid userId;
            if (!Guid.TryParse(evt.userId, out userId))
            {
                logger.LogError("Failed to soft delete user. {UserId} is not a valid guid", evt.userId);
                return Result.Fail("User id is not a valid guid");
            }
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning("User with id {UserId} not found for soft deletion", userId);
                return Result.Fail("User not found");
            }

            user.SoftDelete();
            await unitOfWork.SaveChangesAsync();
            
            logger.LogInformation("User {UserId} soft deleted successfully from event", user.Id);
            return Result.Ok();
        }
    }
}
