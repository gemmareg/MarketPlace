using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Relaciones
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment? Pago { get; set; }

        private Order() { }

        public static Result<Order> Create(User user, List<CartItem> cartItems)
        {
            if (user is null)
                return Result<Order>.Fail(ErrorMessages.INVALID_USER_FOR_ORDER);

            if (cartItems == null || cartItems.Count == 0)
                return Result<Order>.Fail(ErrorMessages.EMPTY_CART_ITEMS);
            var order = new Order()
            {
                UserId = user.Id,
                User = user,
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cartItems)
            {
                var result = OrderItem.Create(cartItem, order);
                if (!result.Success || result.Data is null)
                    return Result<Order>.Fail(result.Message ?? "Error al crear OrderItem desde CartItem.");

                order.OrderItems.Add(result.Data);
            }

            order.Total = order.CalculateTotal();

            return Result<Order>.Ok(order);
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            if (OrderItems != null)
            {
                foreach (var item in OrderItems)
                {
                    total += item.UnitPrice * item.Quantity;
                }
            }
            return total;
        }

        public enum OrderStatus
        {
            Pending,
            Paid,
            Sent,
            Delivered,
            Canceled
        }
    }
}
