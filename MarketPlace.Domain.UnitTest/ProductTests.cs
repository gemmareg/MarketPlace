using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain.UnitTest;

public class ProductTests
{
    private readonly Category _category;
    private readonly User _seller;
    private const string TEST_NAME = "Cacahuetes Tostados Naturales – 1 kg";
    private const string TEST_DESCRIPTION = @"Cacahuetes tostados de alta calidad, seleccionados y tostados de forma uniforme para ofrecer un sabor natural y una textura crujiente. Sin sal añadida ni conservantes, ideales para quienes buscan un snack sencillo y versátil.

Aptos para consumir solos o como ingrediente en ensaladas, platos salteados, repostería o para preparar mantequilla de cacahuete casera. Presentados en bolsa resellable para conservar mejor su frescura.

Un básico práctico y sabroso para el día a día.";
    public ProductTests()
    {
        _category = Category.Create("Category A", "Category's description").Data!;
        _seller = User.Create(Guid.NewGuid(), "User A").Data!;
    }

    [Fact]
    public void Create_WithNullCategory_ReturnsInvalidCategoryError()
    {
        //Arrange
        DateTime date = DateTime.Today;

        //Act
        var result = Product.Create(null, _seller, TEST_NAME, TEST_DESCRIPTION, 10.0m, 100, date, Product.ProductState.Active);

        //Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.INVALID_CATEGORY_FOR_PRODUCT, result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Create_WithNullSeller_ReturnsInvalidSellerError()
    {
        //Arrange
        DateTime date = DateTime.Today;

        //Act
        var result = Product.Create(_category, null, TEST_NAME, TEST_DESCRIPTION, 10.0m, 100, date, Product.ProductState.Active);

        //Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.INVALID_SELLER_FOR_PRODUCT, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [MemberData(nameof(CreateProductCases))]
    public void CreateShouldReturnExpectedResult(
        string name,
        string description,
        decimal price,
        int stock,
        int daysToAddToToday,
        Product.ProductState state,
        bool expectedResult,
        string expectedMessage)
    {
        //Arrange
        DateTime date = DateTime.Today.AddDays(daysToAddToToday);

        //Act
        var result = Product.Create(_category, _seller, name, description, price, stock, date, state);

        //Assert
        if (!expectedResult)
        {
            Assert.False(result.Success);
            Assert.Equal(expectedMessage, result.Message);
            Assert.Null(result.Data);
        }
        else
        {
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(_category.Id, result.Data!.CategoryId);
            Assert.Equal(_seller.Id, result.Data!.SellerId);
            Assert.Equal(name, result.Data.Name);
            Assert.Equal(description, result.Data.Description);
            Assert.Equal(price, result.Data.Price);
            Assert.Equal(stock, result.Data.Stock);
            Assert.Equal(date, result.Data.CreatedDate);
            Assert.Equal(state, result.Data.State);
        }
    }

    public static IEnumerable<object[]> CreateProductCases => new[]
    {
        // Success case
        new object[] {  TEST_NAME, TEST_DESCRIPTION, 10.0m, 100, -1, Product.ProductState.Active, true, string.Empty },

        // Validation failures
        [null, TEST_DESCRIPTION, 10.0, 100, -1, Product.ProductState.Active, false, ErrorMessages.INVALID_PRODUCT_NAME],
        [TEST_NAME, TEST_DESCRIPTION, -20.0, 100, -1, Product.ProductState.Active, false, ErrorMessages.INVALID_PRODUCT_PRICE ],
        [TEST_NAME, TEST_DESCRIPTION, 10.0, -99, -1, Product.ProductState.Active, false, ErrorMessages.INVALID_PRODUCT_STOCK],
    };
}

