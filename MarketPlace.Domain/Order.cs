using System.Collections.Generic;
using System;

namespace MarketPlace.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public User? Usuario { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "pendiente"; // pendiente, pagado, enviado, etc.

        // Relaciones
        public ICollection<OrderItem>? Detalle { get; set; }
        public Payment? Pago { get; set; }
    }
}
