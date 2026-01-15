using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class DeliverOrderCommand : IRequest<Result>
    {
        public string OrderId { get; set; }
    }
}
