using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler(IOrderService orderService) : IRequestHandler<GetOrdersListQuery, Result<List<OrderResumedDto>>>
    {
        public async Task<Result<List<OrderResumedDto>>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            return await orderService.GetAllOrdersAsync(request.CustomerId, request.FromDate, request.ToDate);
        }
    }
}