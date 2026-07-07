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

        public static OrderItemDto ToOrderItemDto(this OrderItem orderItem) => new()
        {
            ProductId = orderItem.Product.Id.ToString(),
            ProductName = orderItem.Product.Name,
            Price = orderItem.UnitPrice,
            Quantity = orderItem.Quantity,
            TotalPrice = orderItem.UnitPrice * orderItem.Quantity
        };

        public static OrderDto ToOrderDto(this Order order) => new()
        {
            Id = order.Id.ToString(),
            Items = [..order.OrderItems.Select(oi => oi.ToOrderItemDto())],
            OrderDate = order.CreatedDate,
            TotalAmount = order.Total,
            Status = order.Status.ToString()
        };
    }
}
