using MarketPlace.Shared;
using System;

namespace MarketPlace.Domain
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        private OrderItem() { }

        public static Result<OrderItem> Create(CartItem cartItem, Order order)
        {
            if (order == null)
                return Result<OrderItem>.Fail("No se pudo crear el item del pedido.");

            if (cartItem == null)
                return Result<OrderItem>.Fail("El item del carrito es inválido.");

            var orderItem = new OrderItem();          
                        
            orderItem.Order = order;
            orderItem.OrderId = order.Id;
            
            orderItem.ProductId = cartItem.ProductId;
            orderItem.Quantity = cartItem.Quantity;
            orderItem.UnitPrice = cartItem.Product?.Price ?? 0m;

            return Result<OrderItem>.Ok(orderItem);
        }
    }
}
