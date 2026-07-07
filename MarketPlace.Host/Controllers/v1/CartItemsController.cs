using Auth.Contracts.Extensions;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.CartItems.Commands.CreateCartItem;
using MarketPlace.Application.Features.CartItems.Commands.DeleteCartItem;
using MarketPlace.Application.Features.CartItems.Commands.UpdateItem;
using MarketPlace.Application.Features.CartItems.Queries.GetCartItems;
using MarketPlace.Host.Dto;
using MarketPlace.Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MarketPlace.Host.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartItemsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<CartItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems()
        {
            var command = new GetCartItemsQuery
            {
                UserId = User.GetId()
            };
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCartItem([FromBody] CreateCartItemRequest request)
        {
            var command = new CreateCartItemCommand
            {
                UserId = User.GetId(),
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCartItem(UpdateCartItemRequest request)
        {
            var command = new UpdateCartItemCommand
            {
                UserId = User.GetId(),
                CartItemId = request.CartItemId,
                Quantity = request.Quantity
            };
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            var command = new DeleteCartItemCommand
            {
                UserId = User.GetId(),
                CartItemId = id
            };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
