using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using System;

namespace MarketPlace.Domain
{
    public class Review : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

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
