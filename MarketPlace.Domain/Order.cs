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

        public decimal Total { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        // Relaciones
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; private set; }

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
                var result = order.CreateItem(cartItem);
                if (!result.Success || result.Data is null)
                    return Result<Order>.Fail(result.Message ?? ErrorMessages.ERROR_CREATING_ORDER_ITEM);

                order.OrderItems.Add(result.Data);
            }

            order.Total = order.CalculateTotal();
            order.CreatePayment();

            return Result<Order>.Ok(order);
        }

        private Result CreatePayment()
        {
            if (Payment != null)
                return Result.Fail(ErrorMessages.PAYMENT_ALREADY_EXISTS);

            Payment = new Payment(Id, Total, PaymentMethod.Undefined);
            return Result.Ok();
        }

        private Result<OrderItem> CreateItem(CartItem cartItem)
        {
            if (cartItem == null)
                return Result<OrderItem>.Fail(ErrorMessages.INVALID_CART_ITEM_FOR_ORDER_ITEM);

            var orderItem = new OrderItem(Id, cartItem.ProductId, cartItem.Quantity, cartItem.Product?.Price ?? 0m);          

            return Result<OrderItem>.Ok(orderItem);
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

        public Result MarkAsPaid(PaymentMethod paymentMethod)
        {
            if (Status != OrderStatus.Pending)
                return Result.Fail(ErrorMessages.ORDER_CANNOT_BE_PAID);
            if(Payment == null)
                return Result.Fail(ErrorMessages.PAYMENT_NOT_CREATED_YET);

            Status = OrderStatus.Paid;
            Payment.SetAsCompleted(paymentMethod);
            return Result.Ok();
        }

        public Result Send()
        {
            if (Status != OrderStatus.Paid)
                return Result.Fail(ErrorMessages.ORDER_CANNOT_BE_SHIPPED);

            Status = OrderStatus.Sent;
            return Result.Ok();
        }

        public Result Deliver()
        {
            if (Status != OrderStatus.Sent)
                return Result.Fail(ErrorMessages.ORDER_CANNOT_BE_DELIVERED);

            Status = OrderStatus.Delivered;
            return Result.Ok();
        }

        public Result Cancel()
        {
            if (Status == OrderStatus.Sent || Status == OrderStatus.Delivered)
                return Result.Fail(ErrorMessages.ORDER_CANNOT_BE_CANCELLED);
            if(Status == OrderStatus.Cancelled)
                return Result.Fail(ErrorMessages.ORDER_ALREADY_CANCELLED);

            Status = OrderStatus.Cancelled;
            return Result.Ok();
        }
    }
}
