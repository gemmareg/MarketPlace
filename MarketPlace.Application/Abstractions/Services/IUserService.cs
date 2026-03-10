using MarketPlace.Application.Dtos.Events;
using MarketPlace.Shared.Result.NonGeneric;

namespace MarketPlace.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result> CreateUserFromEvent(UserRegisteredEvent evt);
    }
}
