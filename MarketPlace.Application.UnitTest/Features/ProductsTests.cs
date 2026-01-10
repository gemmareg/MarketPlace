using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Commands.DeleteProduct;
using MarketPlace.Application.Features.Products.Commands.UpdateProduct;
using MarketPlace.Application.Features.Products.Queries.GetProductById;
using MarketPlace.Application.Features.Products.Queries.GetProductsList;
using MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.Generic;
using Moq;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.UnitTest.Features
{
    public class ProductsTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapper;

        private readonly CreateProductCommandHandler _createProductCommandHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;
        
        private readonly GetProductsListQueryHandler _getProductsListQueryHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
        private readonly GetProductsListByCategoryQueryHandler _getProductsListByCategoryQueryHandler;

        public ProductsTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>(); 
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();            
            _mapper = new Mock<IMapper>();

            _createProductCommandHandler = new CreateProductCommandHandler(
                _productRepositoryMock.Object, 
                _categoryRepositoryMock.Object, 
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _updateProductCommandHandler = new UpdateProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _deleteProductCommandHandler = new DeleteProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _getProductsListQueryHandler = new GetProductsListQueryHandler(
                _productRepositoryMock.Object,
                _mapper.Object);

            _getProductByIdQueryHandler = new GetProductByIdQueryHandler(
                _productRepositoryMock.Object,
                _mapper.Object);

            _getProductsListByCategoryQueryHandler = new GetProductsListByCategoryQueryHandler(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _mapper.Object);
        }

        #region INSERT PRODUCTS
        [Fact]
        public async Task Handle_CreateProduct_Success()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;

            _productRepositoryMock.Setup(pr => pr.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _categoryRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category);
            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);


            // Act
            var result = await _createProductCommandHandler.Handle(
                new CreateProductCommand
                {
                    Name = "Test Product",
                    Description = "Test Description",
                    Price = 100,
                    CategoryId = Guid.NewGuid(),
                    SellerId = Guid.NewGuid(),
                    Stock = 10
                },
                CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateProduct_CategoryNotFound()
        {
            // Arrange
            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category?)null);

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
                Stock = 10,
                CategoryId = Guid.NewGuid(),
                SellerId = Guid.NewGuid()
            };

            // Act
            var result = await _createProductCommandHandler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoría no existe", result.Message);

            _productRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Product>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }


        [Fact]
        public async Task Handle_CreateProduct_UserNotFound()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;

            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(category);

            _userRepositoryMock
                .Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((User?)null);

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
                Stock = 10,
                CategoryId = Guid.NewGuid(),
                SellerId = Guid.NewGuid()
            };

            // Act
            var result = await _createProductCommandHandler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El usuario no existe", result.Message);

            _productRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Product>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
        #endregion

        #region UPDATE PRODUCTS
        [Fact]
        public async Task Handle_UpdateProduct_Success()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var product = Product.Create(category, user, "Old Name", "Old Description", 50, 5, DateTime.Now, ProductState.Inactive).Data!;

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(pr => pr.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateProductCommand
            {
                Id = product.Id,
                Name = "New Name",
                Description = "New Description",
                Price = 100,
                Stock = 10
            };

            // Act
            var result = await _updateProductCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);

            Assert.Equal("New Name", product.Name);
            Assert.Equal("New Description", product.Description);
            Assert.Equal(100, product.Price);
            Assert.Equal(10, product.Stock);

            _productRepositoryMock.Verify(
                repo => repo.UpdateAsync(product),
                Times.Once);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateProduct_ProductNotFound()
        {
            // Arrange
            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product?)null);

            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "New Name",
                Description = "New Description",
                Price = 100,
                Stock = 10
            };

            // Act
            var result = await _updateProductCommandHandler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El producto no existe", result.Message);

            _productRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Product>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
        #endregion

        #region DELETE PRODUCTS
        [Fact]
        public async Task Handle_DeleteProduct_Success()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(pr => pr.RemoveAsync(product))
                .Returns(Task.CompletedTask);

            var command = new DeleteProductCommand
            {
                Id = product.Id
            };

            // Act
            var result = await _deleteProductCommandHandler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.True(result.Success);

            _productRepositoryMock.Verify(
                repo => repo.RemoveAsync(product),
                Times.Once);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteProduct_ProductNotFound()
        {
            // Arrange
            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product?)null);

            var command = new DeleteProductCommand
            {
                Id = Guid.NewGuid()
            };

            // Act
            var result = await _deleteProductCommandHandler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El producto no existe.", result.Message);

            _productRepositoryMock.Verify(
                repo => repo.RemoveAsync(It.IsAny<Product>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
        #endregion

        #region GET PRODUCTS
        [Fact]
        public async Task Handle_GetProductsList_Success()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var products = new List<Product> 
            {
                Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!,
                Product.Create(category, user, "Test Product 2", "Test Description 2", 100, 10, DateTime.Now, ProductState.Active).Data!
            };

            var productDtos = new List<ProductDto> { new(), new() };

            _productRepositoryMock
                .Setup(r => r.GetProductsByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(products);

            _mapper
                .Setup(m => m.Map<List<ProductDto>>(products))
                .Returns(productDtos);

            // Act
            var result = await _getProductsListQueryHandler.Handle(
                new GetProductsListQuery { Search = "test" },
                CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count);
        }

        [Fact]
        public async Task Handle_GetProductsListCommand_ProductNotFound()
        {
            // Arrange
            _productRepositoryMock
                .Setup(r => r.GetProductsByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _getProductsListQueryHandler.Handle(
                new GetProductsListQuery { Search = "test" },
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No products found matching the search criteria.", result.Message);
        }

        [Fact]
        public async Task Handle_GetProductsListByCategory_Success()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var products = new List<Product>
            {
                Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!
            };
            var productDtos = new List<ProductDto> { new() };

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);
            _productRepositoryMock.Setup(r => r.GetProductsByCategoryIdAsync(category.Id)).ReturnsAsync(products);
            _mapper.Setup(m => m.Map<List<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _getProductsListByCategoryQueryHandler.Handle(
                new GetProductsListByCategoryQuery { CategoryId = category.Id.ToString() },
                CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task Handle_GetProductsListByCategory_CategoryNotFound()
        {
            // Arrange
            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            // Act
            var result = await _getProductsListByCategoryQueryHandler.Handle(
                new GetProductsListByCategoryQuery { CategoryId = Guid.NewGuid().ToString() },
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Category not found.", result.Message);
        }

        [Fact]
        public async Task Handle_GetProductsListByCategory_ProductsNotFound()
        {
            // Arrange
            var category = Category.Create("Electronics", "Desc").Data!;

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);
            _productRepositoryMock.Setup(r => r.GetProductsByCategoryIdAsync(category.Id)).ReturnsAsync(new List<Product>());

            // Act
            var result = await _getProductsListByCategoryQueryHandler.Handle(new GetProductsListByCategoryQuery { CategoryId = category.Id.ToString() },CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No products found in the specified category.", result.Message);
        }

        [Fact]
        public async Task Handle_GetProductById_Success()
        {
            // Arrange
            var category = Category.Create("Electronics", "Electronic devices").Data!;
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
            var productDtos = new List<ProductDto> { new() };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
            _mapper.Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>())).Returns(productDtos);

            // Act
            var result = await _getProductByIdQueryHandler.Handle(
                new GetProductByIdQuery { ProductId = product.Id.ToString() },
                CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task Handle_GetProductById_ProductNotFound()
        {
            // Arrange
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _getProductByIdQueryHandler.Handle(
                new GetProductByIdQuery { ProductId = Guid.NewGuid().ToString() },
                CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product not found.", result.Message);
        }
        #endregion
    }
}
