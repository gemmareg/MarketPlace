using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly IOrderService _orderService;

        public CancelOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.CancelOrder(request.OrderId);
        }
    }
}
