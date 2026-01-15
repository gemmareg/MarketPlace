using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public List<string> CartItemIds { get; set; }
    }
}
