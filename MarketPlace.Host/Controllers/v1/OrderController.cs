using Auth.Authorization.Attributes;
using Auth.Contracts.Extensions;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Orders.Commands.CancelOrder;
using MarketPlace.Application.Features.Orders.Commands.CreateOrder;
using MarketPlace.Application.Features.Orders.Commands.DeliverOrder;
using MarketPlace.Application.Features.Orders.Commands.MarkOrderAsPaid;
using MarketPlace.Application.Features.Orders.Commands.Send;
using MarketPlace.Application.Features.Orders.Queries.GetOrderById;
using MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser;
using MarketPlace.Application.Features.Orders.Queries.GetOrdersList;
using MarketPlace.Host.Dto;
using MarketPlace.Host.Extensions;
using MarketPlace.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MarketPlace.Host.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> GetOrderById(string id)
        {
            var command = new GetOrderByIdQuery
            {
                UserId = User.GetId(),
                OrderId = id,
                HasReadAnyPermission = User.HasPermission(MarketPlacePermissions.OrdersReadAny)
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("ByUser")]
        [Authorize]
        [ProducesResponseType(typeof(List<OrderResumedDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderResumedDto>>> GetOrdersByUser()
        {
            var command = new GetOrdersByUserIdQuery
            {
                UserId = User.GetId()
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet]
        [Authorize]
        [RequiresPermission(MarketPlacePermissions.OrdersReadAny)]
        [ProducesResponseType(typeof(List<OrderResumedDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderResumedDto>>> GetAllOrders([FromQuery] string? customerId, DateTime? from, DateTime? to)
        {
            var query = new GetOrdersListQuery
            {
                CustomerId = customerId,
                FromDate = from,
                ToDate = to
            };
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] List<string> cartItems)
        {
            var command = new CreateOrderCommand
            {
                UserId = User.GetId(),
                CartItemIds = cartItems
            };

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("Cancel/{orderId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancel(string orderId)
        {
            var command = new CancelOrderCommand
            {
                UserId = User.GetId(),
                OrderId = orderId
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("Pay/{orderId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Pay(string orderId, [FromBody] MarkOrderAsPaidRequest request)
        {
            var command = new MarkOrderAsPaidCommand
            {
                OrderId = orderId,
                PaymentMethod = request.PaymentMethod
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("Send/{orderId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Send(string orderId)
        {
            var command = new SendOrderCommand
            {
                UserId = User.GetId(),
                OrderId = orderId
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("Deliver/{orderId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deliver(string orderId)
        {
            var command = new DeliverOrderCommand
            {
                OrderId = orderId
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
