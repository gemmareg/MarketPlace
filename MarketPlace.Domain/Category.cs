using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Relaciones
        public ICollection<Product>? Productos { get; set; }
    }

}
