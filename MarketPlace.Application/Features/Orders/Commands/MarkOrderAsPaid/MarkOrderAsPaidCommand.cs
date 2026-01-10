using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.MarkOrderAsPaid
{
    public class MarkOrderAsPaidCommand : IRequest<Result>
    {
        public string OrderId { get; set; }
        public int PaymentMethod { get; set; }
    }
}
