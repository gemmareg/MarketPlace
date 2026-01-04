using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class ProductRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IProductRepository _productRepository;
        
        private readonly User _user;
        private readonly Category _category;
        private readonly Product _product;

        public ProductRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _productRepository = new ProductRepository(_fixture.DbContext);

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
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_User()
        {
            // Act - Insert
            await _productRepository.AddAsync(_product);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Insert
            var insertedProduct = await _productRepository.GetByIdAsync(_product.Id);
            insertedProduct.Should().NotBeNull();
            insertedProduct!.Name.Should().Be("Test Product");

            // Act - Read
            var allProducts = await _productRepository.GetAllAsync();

            // Assert - Read
            allProducts.Should().ContainSingle(u => u.Id == _product.Id);

            // Arrange Update
            insertedProduct.UpdateName("Updated Product Name");
            insertedProduct.UpdateDescription("Updated Description");
            insertedProduct.UpdatePrice(19.99m);
            insertedProduct.UpdateStock(50);

            // Act - Update
            await _productRepository.UpdateAsync(insertedProduct);

            // Assert - Update
            var updatedProduct = await _productRepository.GetByIdAsync(_product.Id);
            updatedProduct!.Name.Should().Be("Updated Product Name");
            updatedProduct.Description.Should().Be("Updated Description");
            updatedProduct.Price.Should().Be(19.99m);

            // Act - Delete
            await _productRepository.RemoveAsync(_product);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Delete
            var deletedUser = await _productRepository.GetByIdAsync(_product.Id);
            deletedUser.Should().BeNull();
        }

    }
}
