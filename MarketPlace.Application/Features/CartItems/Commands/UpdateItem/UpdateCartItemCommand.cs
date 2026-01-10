using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Commands.UpdateItem
{
    public class UpdateCartItemCommand : IRequest<Result>
    {
        public string CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
