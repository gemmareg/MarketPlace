using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
