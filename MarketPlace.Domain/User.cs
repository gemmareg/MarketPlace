using System.Collections.Generic;
using System;

namespace MarketPlace.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "comprador"; // comprador / vendedor / admin
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<Product>? Productos { get; set; }
        public ICollection<Order>? Pedidos { get; set; }
        public ICollection<Review>? Reseñas { get; set; }
        public ICollection<CartItem>? Carrito { get; set; }
    }
}
