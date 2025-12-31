using MarketPlace.Domain.Common;
using System;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethods PaymentMethod { get; set; } = PaymentMethods.DebitCard;
        public PaymentState State { get; set; } = PaymentState.Pending; // pendiente, completado, fallido
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    }
}
