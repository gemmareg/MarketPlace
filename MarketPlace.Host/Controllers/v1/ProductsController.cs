using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Queries.GetProductsList;
using MarketPlace.Host.Abstractions.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MarketPlace.Host.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{search}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(string search)
        {
            var query = new GetProductsListQuery { Search = search };
            try
            {
                var result = await _mediator.Send(query);

                if (!result.Success) return BadRequest(result.Message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            if (!Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var sellerId))
                return BadRequest("SellerId inválido o no autenticado.");
            command.SellerId = sellerId;

            var result = await _mediator.Send(command);

            if (!result.Success) return BadRequest(result.Message);

            return Ok();
        }
    }
}
