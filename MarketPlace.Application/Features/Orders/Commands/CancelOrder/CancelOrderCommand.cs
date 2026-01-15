using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest<Result>
    {
        public string OrderId { get; set; }
    }
}
