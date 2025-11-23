using System.Collections.Generic;
using System;
using MarketPlace.Shared;

namespace MarketPlace.Domain
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public User Usuario { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        // Relaciones
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment? Pago { get; set; }

        private Order() { }

        public static Result<Order> Create(User user, List<CartItem> cartItems)
        {
            if (user is null)
                return Result<Order>.Fail("El usuario no puede ser nulo");

            if (cartItems == null || cartItems.Count == 0)
                return Result<Order>.Fail("La lista del carrito no puede estar vacía.");

            var order = new Order()
            {
                UsuarioId = user.Id,
                Usuario = user,
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

        public enum EstadoPedido
        {
            Pendiente,
            Pagado,
            Enviado,
            Entregado,
            Cancelado
        }
    }
}
