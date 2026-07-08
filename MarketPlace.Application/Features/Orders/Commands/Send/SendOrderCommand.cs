using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class SendOrderCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
    }
}
