using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Categories.Queries.GetCategoryList
{
    public class GetCategoryListRequest : IRequest<Result<List<CategoryDto>>>
    {
    }
}
