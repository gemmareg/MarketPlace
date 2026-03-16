using Auth.Contracts.Events;
using MarketPlace.Shared.Result.NonGeneric;

namespace MarketPlace.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result> CreateUserFromEvent(UserRegisteredEvent evt);

        Task<Result> SoftDeleteUserFromEvent(UserSoftDeletedEvent evt);
    }
}
