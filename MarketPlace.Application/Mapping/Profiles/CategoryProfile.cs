using AutoMapper;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;

namespace MarketPlace.Application.Mapping.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
