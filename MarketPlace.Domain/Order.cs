using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using System;
using System.Collections.Generic;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
        public decimal Total { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

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
                    return Result<Order>.Fail(result.Message ?? ErrorMessages.ERROR_CREATING_ORDER_ITEM);

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

        public Result MarkAsPaid()
        {
            if (Status != OrderStatus.Pending)
                return Result.Fail(ErrorMessages.ORDER_CANNOT_BE_PAID);

            Status = OrderStatus.Paid;
            return Result.Ok();
        }
    }
}
