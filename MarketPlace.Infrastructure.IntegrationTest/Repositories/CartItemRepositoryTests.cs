using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class CartItemRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICartItemRepository _cartItemRepository;

        private readonly User _user;
        private readonly Category _category;
        private readonly Product _product;
        private readonly CartItem _cartItem;

        private const int QUANTITY = 2;
        private const int NEW_QUANTITY = 5;

        public CartItemRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _cartItemRepository = new CartItemRepository(_fixture.DbContext);

            _category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            _user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            _product = Product.Create(
                category: _category,
                seller: _user,
                name: "Test Product",
                description: "A product for testing",
                price: 9.99m,
                stock: 100,
                dateCreated: DateTime.UtcNow,
                state: Product.ProductState.Active).Data!;
            _cartItem = CartItem.Create(
                userId: _user.Id,
                productId: _product.Id,
                quantity: QUANTITY).Data!;

            _fixture.DbContext.Categories.Add(_category);
            _fixture.DbContext.Users.Add(_user);
            _fixture.DbContext.Products.Add(_product);
            _fixture.DbContext.SaveChanges();
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_CartItemt()
        {
            // Act - Insert
            await _cartItemRepository.AddAsync(_cartItem);
            _fixture.DbContext.SaveChanges();

            // Assert - Insert
            var insertedCartItem = await _cartItemRepository.GetByIdAsync(_cartItem.Id);
            insertedCartItem.Should().NotBeNull();

            // Act - Read
            var allCartItems = await _cartItemRepository.GetAllAsync();

            // Assert - Read
            allCartItems.Should().ContainSingle(u => u.Id == _cartItem.Id);

            // Arrange Update
            insertedCartItem!.UpdateQuantity(NEW_QUANTITY);

            // Act - Update
            await _cartItemRepository.UpdateAsync(insertedCartItem);
            _fixture.DbContext.SaveChanges();

            // Assert - Update
            var updatedCartItem = await _cartItemRepository.GetByIdAsync(_cartItem.Id);
            updatedCartItem!.Quantity.Should().Be(NEW_QUANTITY);

            // Act - Delete
            await _cartItemRepository.RemoveAsync(_cartItem);
            _fixture.DbContext.SaveChanges();

            // Assert - Delete
            var deletedCartItem = await _cartItemRepository.GetByIdAsync(_cartItem.Id);
            deletedCartItem.Should().BeNull();
        }
    }
}