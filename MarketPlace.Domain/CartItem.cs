using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
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

        public static Result<CartItem> Create(User user, Product product, int quantity)
        {
            if (user.Id == Guid.Empty) 
                return Result<CartItem>.Fail(ErrorMessages.INVALID_USER);

            if (product.Id == Guid.Empty)
                return Result<CartItem>.Fail(ErrorMessages.INVALID_PRODUCT);

            if(quantity == 0)
                return Result<CartItem>.Fail(ErrorMessages.INVALID_QUANTITY);

            var cartItem = new CartItem();
            cartItem.UserId = user.Id;
            cartItem.ProductId = product.Id;
            cartItem.User = user;
            cartItem.Product = product;
            cartItem.Quantity = quantity;

            return Result<CartItem>.Ok(cartItem);
        }

        public Result UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                return Result.Fail(ErrorMessages.INVALID_QUANTITY);
            Quantity = newQuantity;
            return Result.Ok();
        }

        public Result ValidateStock(Product product)
        {
            if (product.Stock < Quantity)
                return Result.Fail(ErrorMessages.INSUFFICIENT_STOCK);
            return Result.Ok();
        }
    }
}
