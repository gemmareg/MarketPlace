using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Queries.GetCartItems
{
    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, Result<List<CartItemDto>>>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public GetCartItemsQueryHandler(ICartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<CartItemDto>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByUserId(new Guid(request.UserId));
            if (cartItems == null || !cartItems.Any()) return Result<List<CartItemDto>>.Fail("No cart items found for the user.");

            return Result<List<CartItemDto>>.Ok(_mapper.Map<List<CartItemDto>>(cartItems));
        }
    }
}
