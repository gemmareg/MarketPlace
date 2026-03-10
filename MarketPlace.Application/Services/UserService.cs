using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos.Events;
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
            if (!Guid.TryParse(evt.UserId,out userId))
            {
                logger.LogError("Failed to create user. {UserId} is not a valid guid", evt.UserId);
                return Result.Fail("User id is not a valid guid");
            }

            var resultUser = User.Create(userId, evt.Name);
            if (!resultUser.Success)
            {
                logger.LogError("Could not create user: {Message}",resultUser.Message);
                return Result.Fail($"Could not create user: {resultUser.Message}");
            }

            var user = resultUser.Data;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} created successfully from event", user.Id);
            return Result.Ok();
        }
    }
}
