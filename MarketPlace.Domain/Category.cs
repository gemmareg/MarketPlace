using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Relaciones
        public ICollection<Product>? Productos { get; set; }
    }

}
