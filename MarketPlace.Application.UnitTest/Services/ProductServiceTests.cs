using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Commands.UpdateProduct;
using MarketPlace.Application.Services;
using MarketPlace.Domain;
using Moq;

namespace MarketPlace.Application.UnitTest.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly Product _productFixture;
    private readonly User _userFixture;
    private readonly Category _categoryFixture;

    public ProductServiceTests()
    {
        _categoryFixture = Category.Create("Electronics", "").Data;
        _userFixture = User.Create(Guid.NewGuid(), "John").Data;
        _productFixture = Product.Create(
            _categoryFixture,
            _userFixture,
            "Gaming Mouse Logitech G502",
            "Ratón gaming de alta precisión con sensor HERO y 11 botones programables.",
            (decimal)45.5,
            100,
            DateTime.Now,
            MarketPlace.Shared.Enums.ProductState.Active).Data!;
    }

    private ProductService CreateService() =>
        new ProductService(
            _productRepoMock.Object,
            _categoryRepoMock.Object,
            _userRepoMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );

    [Fact]
    public async Task GetProductsList_ShouldReturnFail_WhenNoProductsFound()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetProductsByNameAsync("abc"))
            .ReturnsAsync(new List<Product>());

        var service = CreateService();

        // Act
        var result = await service.getProductsList("abc");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found matching the search criteria.", result.Message);
    }

    [Fact]
    public async Task GetProductsList_ShouldReturnOk_WhenProductsExist()
    {
        // Arrange
        var products = new List<Product> { _productFixture };
        var productsDto = new List<ProductDto> { new ProductDto() };

        _productRepoMock.Setup(r => r.GetProductsByNameAsync("abc"))
            .ReturnsAsync(products);

        _mapperMock.Setup(m => m.Map<List<ProductDto>>(products))
            .Returns(productsDto);

        var service = CreateService();

        // Act
        var result = await service.getProductsList("abc");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(productsDto, result.Data);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnFail_WhenProductNotFound()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);

        var service = CreateService();

        var result = await service.GetProductById(Guid.NewGuid().ToString());

        Assert.False(result.Success);
        Assert.Equal("Product not found.", result.Message);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnOk_WhenProductExists()
    {
        var dto = new ProductDto();

        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_productFixture);

        _mapperMock.Setup(m => m.Map<ProductDto>(_productFixture))
            .Returns(dto);

        var service = CreateService();

        var result = await service.GetProductById(Guid.NewGuid().ToString());

        Assert.True(result.Success);
        Assert.Equal(dto, result.Data);
    }

    [Fact]
    public async Task CreateProduct_ShouldFail_WhenSellerIdIsInvalid()
    {
        var service = CreateService();

        var request = new CreateProductCommand { SellerId = "invalid-guid" };

        var result = await service.CreateProduct(request);

        Assert.False(result.Success);
        Assert.Equal("Usuario autenticado inválido.", result.Message);
    }

    [Fact]
    public async Task CreateProduct_ShouldFail_WhenSellerDoesNotExist()
    {
        var request = new CreateProductCommand
        {
            SellerId = Guid.NewGuid().ToString(),
            CategoryId = Guid.NewGuid().ToString()
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User)null);

        var service = CreateService();

        var result = await service.CreateProduct(request);

        Assert.False(result.Success);
        Assert.Equal("El usuario no existe", result.Message);
    }

    [Fact]
    public async Task CreateProduct_ShouldFail_WhenCategoryDoesNotExist()
    {
        var request = new CreateProductCommand
        {
            SellerId = Guid.NewGuid().ToString(),
            CategoryId = Guid.NewGuid().ToString()
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_userFixture);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Category)null);

        var service = CreateService();

        var result = await service.CreateProduct(request);

        Assert.False(result.Success);
        Assert.Equal("La categoría no existe", result.Message);
    }

    [Fact]
    public async Task DeleteProduct_ShouldFail_WhenProductNotFound()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);

        var service = CreateService();

        var result = await service.DeleteProduct(Guid.NewGuid().ToString(), _userFixture.Id.ToString(), false);

        Assert.False(result.Success);
        Assert.Equal("El producto no existe.", result.Message);
    }

    [Fact]
    public async Task DeleteProduct_ShouldSucceed_WhenProductExists()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_productFixture);

        var service = CreateService();

        var result = await service.DeleteProduct(Guid.NewGuid().ToString(), _userFixture.Id.ToString(), false);

        Assert.True(result.Success);
        _productRepoMock.Verify(r => r.RemoveAsync(_productFixture), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldFail_WhenProductNotFound()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);

        var service = CreateService();

        var result = await service.UpdateProduct(new UpdateProductCommand { Id = Guid.NewGuid().ToString() });

        Assert.False(result.Success);
        Assert.Equal("El producto no existe", result.Message);
    }

    [Fact]
    public async Task UpdateProduct_ShouldSucceed_WhenProductExists()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_productFixture);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_categoryFixture);

        var service = CreateService();

        var result = await service.UpdateProduct(new UpdateProductCommand
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Nuevo",
            Description = "Desc",
            Price = 10,
            Stock = 5,
            UserId = _userFixture.Id.ToString(),
            CategoryId = _categoryFixture.Id.ToString(),
        });

        Assert.True(result.Success);
        _productRepoMock.Verify(r => r.UpdateAsync(_productFixture), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}