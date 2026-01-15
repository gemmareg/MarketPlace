using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class ReviewRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IReviewRepository _reviewRepository;

        private readonly User _user;
        private readonly Category _category;
        private readonly Product _product;
        private readonly Review _review;
        private readonly User _reviewer;

        public ReviewRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _reviewRepository = new ReviewRepository(_fixture.DbContext);

            _category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            _user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            _reviewer = User.Create(Guid.NewGuid(), "Jane Smith").Data!;
            _product = Product.Create(
                category: _category,
                seller: _user,
                name: "Test Product",
                description: "A product for testing",
                price: 9.99m,
                stock: 100,
                dateCreated: DateTime.UtcNow,
                state: ProductState.Active).Data!;
            _review = Review.Create(_reviewer, _product, 5, "Great product!").Data!;
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_User()
        {
            // Act - Insert
            await _reviewRepository.AddAsync(_review);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Insert
            var insertedReview = await _reviewRepository.GetByIdAsync(_review.Id);
            insertedReview.Should().NotBeNull();
            insertedReview!.Comment.Should().Be("Great product!");

            // Act - Read
            var allReviews = await _reviewRepository.GetAllAsync();

            // Assert - Read
            allReviews.Should().ContainSingle(u => u.Id == _review.Id);

            // Act - Delete
            await _reviewRepository.RemoveAsync(_review);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Delete
            var deletedReview = await _reviewRepository.GetByIdAsync(_review.Id);
            deletedReview.Should().BeNull();
        }
    }
}
