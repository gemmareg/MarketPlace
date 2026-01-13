using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Product : BaseEntity
    {
        public Guid CategoryId { get; private set; }
        public Guid SellerId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public ProductState State { get; private set; } = ProductState.Inactive;

        // Relaciones
        public Category Category { get; private set; }
        public User Seller { get; private set; }

        public ICollection<OrderItem>? Products { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<CartItem>? InCart { get; set; }

        public enum ProductState
        {
            Active,
            Inactive,
            SoldOut
        }

        private Product() { }

        public static Result<Product> Create(
            Category category,
            User seller,
            string name,
            string description,
            decimal price,
            int stock,
            DateTime dateCreated,
            ProductState state)
        {
            if (category is null)
                return Result<Product>.Fail(ErrorMessages.INVALID_CATEGORY_FOR_PRODUCT);

            if (seller is null)
                return Result<Product>.Fail(ErrorMessages.INVALID_SELLER_FOR_PRODUCT);

            if (string.IsNullOrEmpty(name))
                return Result<Product>.Fail(ErrorMessages.INVALID_PRODUCT_NAME);

            if (price < 0)
                return Result<Product>.Fail(ErrorMessages.INVALID_PRODUCT_PRICE);

            if (stock < 0)
                return Result<Product>.Fail(ErrorMessages.INVALID_PRODUCT_STOCK);

            if (string.IsNullOrEmpty(description))
                description = string.Empty;

            var product = new Product()
            {
                CategoryId = category.Id,
                SellerId = seller.Id,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                CreatedDate = dateCreated,
                State = state,

                Category = category,
                Seller = seller,

                Products = new List<OrderItem>(),
                Reviews = new List<Review>(),
                InCart = new List<CartItem>()
            };

            return Result<Product>.Ok(product);
        }
    }
}
