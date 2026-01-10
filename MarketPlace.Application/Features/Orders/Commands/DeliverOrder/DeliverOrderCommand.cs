using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.DeliverOrder
{
    public class DeliverOrderCommand : IRequest<Result>
    {
        public string OrderId { get; set; }
    }
}
