using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser
{
    public class GetOrdersByUserIdQuery : IRequest<Result<List<OrderResumedDto>>>
    {
        public string UserId { get; set; }
    }
}
