using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Commands.DeleteProduct;
using MarketPlace.Application.Features.Products.Commands.UpdateProduct;
using MarketPlace.Application.Features.Products.Queries.GetProductById;
using MarketPlace.Application.Features.Products.Queries.GetProductsBySeller;
using MarketPlace.Application.Features.Products.Queries.GetProductsList;
using MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory;
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
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsById(string id)
        {
            var query = new GetProductByIdQuery { ProductId = id };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string search)
        {
            var query = new GetProductsListQuery { Search = search };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [AllowAnonymous]
        [HttpGet("ByCategory/{categoryId}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(string categoryId)
        {
            var query = new GetProductsListByCategoryQuery { CategoryId = categoryId };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("BySeller/{sellerId}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySeller(string sellerId)
        {
            var query = new GetProductsBySellerQuery { SellerId = sellerId };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            request.SellerId = User.GetId();

            var result = await _mediator.Send(request);

            return result.ToActionResult();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Stock = request.Stock,
                State = request.State,
                UserId = User.GetId(),
                IsAdmin = User.IsAdmin()
            };

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand()
            {
                Id = id,
                UserId = User.GetId(),
                IsAdmin = User.IsAdmin()
            };

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
