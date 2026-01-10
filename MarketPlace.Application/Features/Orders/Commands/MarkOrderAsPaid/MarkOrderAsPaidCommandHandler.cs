using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.MarkOrderAsPaid
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, Result>
    {
        private readonly IOrderService _orderService;

        public MarkOrderAsPaidCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.MarkOrderAsPaid(request.OrderId, request.PaymentMethod);
        }
    }
}
