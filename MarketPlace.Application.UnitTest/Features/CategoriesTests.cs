using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Features.Categories.Commands.CreateCategory;
using MarketPlace.Application.Features.Categories.Commands.DeleteCategory;
using MarketPlace.Application.Features.Categories.Commands.UpdateCategory;
using Moq;

namespace MarketPlace.Application.UnitTest.Features
{
    public class CategoriesTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IMapper> _mapperMock;

        private readonly CreateCategoryCommandHandler _createCategoryCommandHandler;
        private readonly UpdateCategoryCommandHandler _updateCategoryCommandHandler;
        private readonly DeleteCategoryCommandHandler _deleteCategoryCommandHandler;

        public CategoriesTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _createCategoryCommandHandler = new CreateCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWork.Object, _mapperMock.Object);
            _updateCategoryCommandHandler = new UpdateCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWork.Object);
            _deleteCategoryCommandHandler = new DeleteCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateCategory_Success()
        {
            // Arrange
            var command = new CreateCategoryCommand()
            {
                Name = "Electronics",
                Description = "Electronic devices and gadgets"
            };

            // Act
            var result = await _createCategoryCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Category>()), Times.Once);
            _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_Success()
        {
            // Arrange
            var existingCategory = Domain.Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            var command = new UpdateCategoryCommand()
            {
                Id = existingCategory.Id.ToString(),
                Name = "Updated Name",
                Description = "Updated Description"
            };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCategory.Id)).ReturnsAsync(existingCategory);

            // Act
            var result = await _updateCategoryCommandHandler.Handle(command, CancellationToken.None);
            
            // Assert
            Assert.True(result.Success);
            _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(existingCategory), Times.Once);
            _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_CategoryNotFound_Failure()
        {
            // Arrange
            var missingId = Guid.NewGuid();
            var command = new UpdateCategoryCommand()
            {
                Id = missingId.ToString(),
                Name = "Updated Name",
                Description = "Updated Description"
            };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(missingId))
                .ReturnsAsync((Domain.Category?)null);

            // Act
            var result = await _updateCategoryCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoría no existe", result.Message);
        }

        [Fact]
        public async Task DeleteCategory_Success()
        {
            // Arrange
            var existingCategory = Domain.Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            var command = new DeleteCategoryCommand()
            {
                Id = existingCategory.Id.ToString()
            };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCategory.Id))
                .ReturnsAsync(existingCategory);

            // Act
            var result = await _deleteCategoryCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            _categoryRepositoryMock.Verify(repo => repo.RemoveAsync(existingCategory), Times.Once);
            _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_CategoryNotFound_Failure()
        {
            // Arrange
            var missingId = Guid.NewGuid();
            var command = new DeleteCategoryCommand()
            {
                Id = missingId.ToString()
            };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(missingId))
                .ReturnsAsync((Domain.Category?)null);
            // Act
            var result = await _deleteCategoryCommandHandler.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoría no existe", result.Message);
        }
    }
}
