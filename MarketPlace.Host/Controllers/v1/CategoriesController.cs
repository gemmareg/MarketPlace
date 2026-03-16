using MarketPlace.Application.Features.Categories.Commands.CreateCategory;
using MarketPlace.Application.Features.Categories.Queries.GetCategoryList;
using MarketPlace.Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Host.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoryListRequest request)
        {
            var result = await _mediator.Send(request);

            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

    }
}
