using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Queries.GetProductsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.Success) return BadRequest(result.Message);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
