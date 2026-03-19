using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Commands.DeleteCartItem
{
    public class DeleteCartItemCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string CartItemId { get; set; }
    }
}
