using System.Collections.Generic;
using System;

namespace MarketPlace.Domain
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.UtcNow;
        public ProductState State { get; set; } = ProductState.Active;

        // Relaciones
        public int CategoryId { get; set; }
        public Category Category{ get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; }

        public ICollection<OrderItem>? Products { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<CartItem>? InCart { get; set; }

        public enum ProductState
        {
            Active,
            Inactive,
            SoldOut
        }
    }
}
