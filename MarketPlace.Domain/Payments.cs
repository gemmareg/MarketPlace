using System;

namespace MarketPlace.Domain
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PedidoId { get; set; }
        public Order? Pedido { get; set; }

        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "tarjeta";
        public string Estado { get; set; } = "pendiente"; // pendiente, completado, fallido
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    }
}
