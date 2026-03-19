using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Extensions.Mapping;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Queries.GetCartItems
{
    public class GetCartItemsQueryHandler(ICartItemRepository cartItemRepository) : IRequestHandler<GetCartItemsQuery, Result<List<CartItemDto>>>
    {
        public async Task<Result<List<CartItemDto>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await cartItemRepository.GetCartItemsByUserId(new Guid(request.UserId));
            if (cartItems == null || !cartItems.Any()) return Result<List<CartItemDto>>.Fail("No cart items found for the user.");

            return Result<List<CartItemDto>>.Ok(cartItems.ToCartItemDto());
        }
    }
}
