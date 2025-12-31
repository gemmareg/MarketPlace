using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using System;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order? Order { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethods PaymentMethod { get; set; } = PaymentMethods.DebitCard;
        public PaymentState State { get; set; } = PaymentState.Pending; // pendiente, completado, fallido
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        private Payment() { }

        public static Result<Payment> Create(Order order, decimal amount, PaymentMethods paymentMethod)
        {
            if(order == null)
                return Result<Payment>.Fail(ErrorMessages.INVALID_ORDER_FOR_PAYMENT);
            if(amount < 0)
                return Result<Payment>.Fail(ErrorMessages.INVALID_PAYMENT_AMOUNT);
            if(!Enum.IsDefined(typeof(PaymentMethods), paymentMethod))
                return Result<Payment>.Fail(ErrorMessages.INVALID_PAYMENT_METHOD);

            var payment = new Payment
            {
                OrderId = order.Id,
                Order = order,
                Amount = amount,
                PaymentMethod = paymentMethod,
                State = PaymentState.Pending,
                FechaPago = DateTime.UtcNow
            };

            return Result<Payment>.Ok(payment);
        }
    }
}
