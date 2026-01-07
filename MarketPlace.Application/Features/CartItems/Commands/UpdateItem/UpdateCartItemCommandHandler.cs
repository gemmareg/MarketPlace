using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Commands.UpdateItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, Result>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemCommandHandler(ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork)
        {
            _cartItemRepository = cartItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var cartItem = _cartItemRepository.GetByIdAsync(new Guid(request.CartItemId)).Result;
            if (cartItem == null) return Result.Fail("Cart item not found.");

            cartItem.UpdateQuantity(request.Quantity);
            await _cartItemRepository.UpdateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
