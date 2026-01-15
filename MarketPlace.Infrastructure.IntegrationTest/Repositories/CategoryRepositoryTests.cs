using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class CategoryRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICategoryRepository _categoryRepository;

        private readonly Category _category;

        private const string NEW_CATEGORY_NAME = "Electronigs and gabgets";
        private readonly string NEW_CATEGORY_DESCRIPTION = "";

        public CategoryRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _categoryRepository = new CategoryRepository(_fixture.DbContext);

            _category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_Category()
        {
            // Act - Insert
            await _categoryRepository.AddAsync(_category);
            _fixture.DbContext.SaveChanges();

            // Assert - Insert
            var insertedCategory = await _categoryRepository.GetByIdAsync(_category.Id);
            insertedCategory.Should().NotBeNull();

            // Act - Read
            var allCartItems = await _categoryRepository.GetAllAsync();

            // Assert - Read
            allCartItems.Should().ContainSingle(u => u.Id == _category.Id);

            // Arrange Update
            insertedCategory.SetName(NEW_CATEGORY_NAME);
            insertedCategory.SetDescription(NEW_CATEGORY_DESCRIPTION);

            // Act - Update
            await _categoryRepository.UpdateAsync(insertedCategory);
            _fixture.DbContext.SaveChanges();

            // Assert - Update
            var updatedCategory = await _categoryRepository.GetByIdAsync(_category.Id);
            updatedCategory.Should().NotBeNull();
            updatedCategory.Name.Should().Be(NEW_CATEGORY_NAME);
            updatedCategory.Description.Should().Be(NEW_CATEGORY_DESCRIPTION);

            // Act - Delete
            await _categoryRepository.RemoveAsync(_category);
            _fixture.DbContext.SaveChanges();

            // Assert - Delete
            var deletedCartItem = await _categoryRepository.GetByIdAsync(_category.Id);
            deletedCartItem.Should().BeNull();
        }
    }
}
