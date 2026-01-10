using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Queries.GetCartItems
{
    public class GetCartItemsQuery : IRequest<Result<List<CartItemDto>>>
    {
        public string UserId { get; set; }
    }
}
