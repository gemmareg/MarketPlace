using MarketPlace.Application.Abstractions.Repositories.Common;
using MarketPlace.Domain;

namespace MarketPlace.Application.Abstractions.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<List<CartItem>> GetByIdsAsync(List<Guid> ids);
        Task<List<CartItem>> GetCartItemsByUserId(Guid userId);
    }
}
