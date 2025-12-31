using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using System;

namespace MarketPlace.Domain
{
    public class Review : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;

        private Review() { }

        public static Result<Review> Create(
            User user,
            Product product,
            int rating,
            string? comment)
        {
            if (user is null)
                return Result<Review>.Fail(ErrorMessages.INVALID_USER_FOR_REVIEW);
            if (product is null)
                return Result<Review>.Fail(ErrorMessages.INVALID_PRODUCT_FOR_REVIEW);
            if (rating < 1 || rating > 5)
                return Result<Review>.Fail(ErrorMessages.INVALID_RATING_FOR_REVIEW);
            
            var review = new Review();
            review.User = user;
            review.UserId = user.Id;
            review.Product = product;
            review.ProductId = product.Id;
            review.Rating = rating;
            review.Comment = comment;
            review.Date = DateTime.UtcNow;

            return Result<Review>.Ok(review);
        }
    }
}
