using MarketPlace.Domain.Common;
using System;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod.DebitCard;
        public PaymentState State { get; private set; } = PaymentState.Pending; // pendiente, completado, fallido
        public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;

        internal Payment(Guid orderId, decimal amount, PaymentMethod paymentMethod)
        {
            OrderId = orderId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            State = PaymentState.Pending;
        }

        internal void SetAsCompleted(PaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
            State = PaymentState.Completed;
            PaymentDate = DateTime.UtcNow;
        }
    }
}
