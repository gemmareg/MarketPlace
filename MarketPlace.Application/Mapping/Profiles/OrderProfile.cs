using AutoMapper;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;

namespace MarketPlace.Application.Mapping.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ReverseMap();
        }
    }
}
