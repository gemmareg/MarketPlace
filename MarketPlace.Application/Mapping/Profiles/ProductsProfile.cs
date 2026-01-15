using AutoMapper;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;

namespace MarketPlace.Application.Mapping.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
