using Azure.Core;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Queries.GetProductsList;
using MarketPlace.Host.Extensions;
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
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            var sellerId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User?.FindFirst("sub")?.Value
                ?? User?.FindFirst("id")?.Value;

            var command = new CreateProductCommand
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Stock = request.Stock,
                SellerId = sellerId,
            };

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
