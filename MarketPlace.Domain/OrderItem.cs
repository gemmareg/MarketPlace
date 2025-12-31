using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using System;

namespace MarketPlace.Domain
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public Order Order { get; private set; }
        public Product Product { get; private set; }

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderItem() { }

        public static Result<OrderItem> Create(CartItem cartItem, Order order)
        {
            if (order == null)
                return Result<OrderItem>.Fail(ErrorMessages.INVALID_ORDER_FOR_ORDER_ITEM);

            if (cartItem == null)
                return Result<OrderItem>.Fail(ErrorMessages.INVALID_CART_ITEM_FOR_ORDER_ITEM);

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
