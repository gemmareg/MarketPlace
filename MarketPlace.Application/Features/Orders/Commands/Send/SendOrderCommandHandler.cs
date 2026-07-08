using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class SendOrderCommandHandler : IRequestHandler<SendOrderCommand, Result>
    {
        private readonly IOrderService _orderService;
        public SendOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<Result> Handle(SendOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.SendOrder(request.OrderId, request.UserId);
        }
    }
}
