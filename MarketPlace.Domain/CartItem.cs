using MarketPlace.Shared;
using System;

namespace MarketPlace.Domain
{
    public class CartItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public User User { get; private set; }
        public Product Product { get; private set; }

        private CartItem() { }

        public static Result<CartItem> Create(Guid userId, Guid productId, int quantity)
        {
            if (userId == Guid.Empty) 
                return Result<CartItem>.Fail("El usuario no es válido");

            if (productId == Guid.Empty)
                return Result<CartItem>.Fail("El producto no es válido");

            if(quantity == 0)
                return Result<CartItem>.Fail("La cantidad no es válida");

            var cartItem = new CartItem();
            cartItem.UserId = userId;
            cartItem.ProductId = productId;
            cartItem.Quantity = quantity;

            return Result<CartItem>.Ok(cartItem);
        }

    }
}
