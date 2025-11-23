using System;

namespace MarketPlace.Domain
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public User Usuario { get; set; }

        public int ProductoId { get; set; }
        public Product Producto { get; set; }

        public int Rating { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
