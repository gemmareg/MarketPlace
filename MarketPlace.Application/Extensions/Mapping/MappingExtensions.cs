using MarketPlace.Application.Dtos;
using MarketPlace.Domain;

namespace MarketPlace.Application.Extensions.Mapping
{
    public static class MappingExtensions
    {
        public static List<CartItemDto> ToCartItemDto(this List<CartItem> cartItems) =>
            [..cartItems.Select(c => new CartItemDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                Price = c.Product.Price,
                Quantity = c.Quantity,
                TotalPrice = c.Product.Price * c.Quantity
            })];
    }
}
