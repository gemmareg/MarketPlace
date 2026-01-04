using MarketPlace.Domain;
using MarketPlace.Shared;

public class ProductTests
{
    private readonly Category _category;
    private readonly User _seller;

    public ProductTests()
    {
        _category = Category.Create("Electrónica", "Dispositivos electrónicos").Data!;
        _seller = User.Create(Guid.NewGuid(), "John Doe").Data!;
    }

    [Fact]
    public void Should_Create_Product_Successfully()
    {
        // Act
        var result = Product.Create(
            _category,
            _seller,
            "Laptop",
            "Laptop para testing",
            1000m,
            5,
            DateTime.UtcNow,
            Product.ProductState.Inactive);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Laptop", result.Data!.Name);
        Assert.Equal(Product.ProductState.Inactive, result.Data.State);
    }

    [Fact]
    public void UpdateName_Should_Change_Name_When_Valid()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "OldName", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;
        var newProductName = "NewName";

        // Act
        var result = product.UpdateName(newProductName);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(newProductName, product.Name);
    }

    [Fact]
    public void UpdateName_Should_Fail_When_Empty()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;

        // Act
        var result = product.UpdateName("");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.INVALID_PRODUCT_NAME, result.Message);
    }

    [Fact]
    public void UpdateDescription_Should_Change_Description()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "OldDesc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;

        // Act
        var result = product.UpdateDescription("NewDesc");

        // Assert
        Assert.True(result.Success);
        Assert.Equal("NewDesc", product.Description);
    }

    [Fact]
    public void UpdatePrice_Should_Change_Price_When_Valid()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;

        // Act
        var result = product.UpdatePrice(20m);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(20m, product.Price);
    }

    [Fact]
    public void UpdatePrice_Should_Fail_When_Negative()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;

        // Act
        var result = product.UpdatePrice(-5m);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.INVALID_PRODUCT_PRICE, result.Message);
    }

    [Fact]
    public void ChangeCategory_Should_Update_Category_When_Valid()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Active).Data!;
        var newCategory = Category.Create("Informática", "PC y accesorios").Data!;

        // Act
        var result = product.ChangeCategory(newCategory);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(newCategory, product.Category);
        Assert.Equal(newCategory.Id, product.CategoryId);
    }

    [Fact]
    public void Activate_And_Deactivate_Should_Change_State()
    {
        // Arrange
        var product = Product.Create(_category, _seller, "Laptop", "Desc", 10m, 1, DateTime.UtcNow, Product.ProductState.Inactive).Data!;

        // Act & Assert
        product.Activate();
        Assert.Equal(Product.ProductState.Active, product.State);

        product.Deactivate();
        Assert.Equal(Product.ProductState.Inactive, product.State);
    }
}
