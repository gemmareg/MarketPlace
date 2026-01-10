using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class DeliverOrderCommandHandler : IRequestHandler<DeliverOrderCommand, Result>
    {
        private readonly IOrderService _orderService;
        public DeliverOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<Result> Handle(DeliverOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.DeliverOrder(request.OrderId);
        }
    }
}
