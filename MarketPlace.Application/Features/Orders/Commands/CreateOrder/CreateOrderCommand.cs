using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<Guid>>
    {
        public string UserId { get; set; }
        public List<string> CartItemIds { get; set; }
    }
}
