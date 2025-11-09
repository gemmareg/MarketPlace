using System.Collections.Generic;
using System;

namespace MarketPlace.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "activo";

        // Relaciones
        public int CategoriaId { get; set; }
        public Category? Categoria { get; set; }

        public int VendedorId { get; set; }
        public User? Vendedor { get; set; }

        public ICollection<OrderItem>? DetallePedidos { get; set; }
        public ICollection<Review>? Reseñas { get; set; }
        public ICollection<CartItem>? EnCarrito { get; set; }
    }
}
