using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using System;

namespace MarketPlace.Domain
{
    public class CartItem : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public User User { get; private set; }
        public Product Product { get; private set; }

        private CartItem() { }

        public static Result<CartItem> Create(Guid userId, Guid productId, int quantity)
        {
            if (userId == Guid.Empty) 
                return Result<CartItem>.Fail(ErrorMessages.INVALID_USER);

            if (productId == Guid.Empty)
                return Result<CartItem>.Fail(ErrorMessages.INVALID_PRODUCT);

            if(quantity == 0)
                return Result<CartItem>.Fail(ErrorMessages.INVALID_QUANTITY);

            var cartItem = new CartItem();
            cartItem.UserId = userId;
            cartItem.ProductId = productId;
            cartItem.Quantity = quantity;

            return Result<CartItem>.Ok(cartItem);
        }

    }
}
