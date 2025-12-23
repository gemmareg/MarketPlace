using MarketPlace.Shared;
using static MarketPlace.Domain.Product;

namespace MarketPlace.Domain.UnitTest
{
    public class ReviewTests
    {
        // user fixtures
        private const string USERNAME = "testuser";
        private const string SELLERNAME = "testseller";
        private const string USEREMAIL = "testuser@test.com";
        private const string SELLEREMAIL = "testuser@seller.com";
        private const string PASSWORD = "hashedpassword";

        // Category fixtures
        private const string CATEGORYNAME = "Electronics";
        private const string CATEGORYDESCRIPTION = "Electronic devices and gadgets";

        // Product fixtures
        private const string PRODUCTNAME = "Smartphone";
        private const string PRODUCTDESCRIPTION = "Latest model smartphone";
        private const decimal PRODUCTPRICE = 699.99m;
        private const int PRODUCTSTOCK = 100;

        // Review fixtures
        private const string VALID_REVIEW = "Great product!";

        [Fact]
        public void Create_ValidParameters_ReturnsSuccess()
        {
            // Arrange
            var user = User.Create(USERNAME, USEREMAIL, PASSWORD).Data!;
            var seller = User.Create(SELLERNAME, SELLEREMAIL, PASSWORD).Data!;
            var category = Category.Create(CATEGORYNAME, CATEGORYDESCRIPTION).Data!;
            var product = Product.Create(category, seller, PRODUCTNAME, PRODUCTDESCRIPTION, PRODUCTPRICE, PRODUCTSTOCK, new DateTime(2023, 11, 22), ProductState.Active).Data!;

            // Act
            var result = Review.Create(user, product, 5, VALID_REVIEW);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(5, result.Data!.Rating);
            Assert.Equal(VALID_REVIEW, result.Data.Comment);
        }

        [Fact]
        public void Create_NullUser_ReturnsFailure()
        {
            // Arrange
            var seller = User.Create(SELLERNAME, SELLEREMAIL, PASSWORD).Data!;
            var category = Category.Create(CATEGORYNAME, CATEGORYDESCRIPTION).Data!;
            var product = Product.Create(category, seller, PRODUCTNAME, PRODUCTDESCRIPTION, PRODUCTPRICE, PRODUCTSTOCK, new DateTime(2023, 11, 22), ProductState.Active).Data!;

            // Act
            var result = Review.Create(null!, product, 5, VALID_REVIEW);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.INVALID_USER_FOR_REVIEW, result.Message);
        }

        [Fact]
        public void Create_NullProduct_ReturnsFailure()
        {
            // Arrange
            var user = User.Create(USERNAME, USEREMAIL, PASSWORD).Data!;
            var seller = User.Create(SELLERNAME, SELLEREMAIL, PASSWORD).Data!;
            var category = Category.Create(CATEGORYNAME, CATEGORYDESCRIPTION).Data!;

            // Act
            var result = Review.Create(user, null!, 5, VALID_REVIEW);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.INVALID_PRODUCT_FOR_REVIEW, result.Message);
        }

        [Fact]
        public void Create_InvalidRating_ReturnsFailure()
        {
            // Arrange
            var user = User.Create(USERNAME, USEREMAIL, PASSWORD).Data!;
            var seller = User.Create(SELLERNAME, SELLEREMAIL, PASSWORD).Data!;
            var category = Category.Create(CATEGORYNAME, CATEGORYDESCRIPTION).Data!;
            var product = Product.Create(category, seller, PRODUCTNAME, PRODUCTDESCRIPTION, PRODUCTPRICE, PRODUCTSTOCK, new DateTime(2023, 11, 22), ProductState.Active).Data!;

            // Act
            var resultLow = Review.Create(user, product, 0, "Bad rating");

            // Assert
            Assert.False(resultLow.Success);
            Assert.Equal(ErrorMessages.INVALID_RATING_FOR_REVIEW, resultLow.Message);
        }
    }
}