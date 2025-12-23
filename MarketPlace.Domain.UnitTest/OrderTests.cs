using MarketPlace.Shared;

namespace MarketPlace.Domain.UnitTest
{
    public class OrderTests
    {
        public const string VALID_USER_NAME = "Ramon";
        public const string VALID_USER_EMAIL = "ramon@test.com";
        public const string INVALID_USER_PASSWORD = "abcd";

        [Fact]
        public void Create_ShouldReturnOkResult_WhenParametersAreValid()
        {
            // Arrange
            var user = User.Create(VALID_USER_NAME, VALID_USER_EMAIL, INVALID_USER_PASSWORD).Data!;
            var cartItems = new List<CartItem>(){
                    CartItem.Create(user.Id, Guid.NewGuid(), 2).Data!,
                    CartItem.Create(user.Id, Guid.NewGuid(), 1).Data!
                };

            // Act
            var result = Order.Create(user, cartItems);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Id, result.Data!.UserId);
            Assert.Equal(cartItems.Count, result.Data.OrderItems.Count);
        }

        [Fact]
        public void Create_ShouldReturnFailResult_WhenUserIsNull()
        {
            // Arrange
            User? user = null;
            var cartItems = new List<CartItem>(){
                    CartItem.Create(Guid.NewGuid(), Guid.NewGuid(), 2).Data!,
                    CartItem.Create(Guid.NewGuid(), Guid.NewGuid(), 1).Data!
                };
            // Act
            var result = Order.Create(user!, cartItems);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.INVALID_USER_FOR_ORDER, result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public void Create_ShouldReturnFailResult_WhenCartItemsIsEmpty()
        {
            // Arrange
            var user = User.Create(VALID_USER_NAME, VALID_USER_EMAIL, INVALID_USER_PASSWORD).Data!;
            var cartItems = new List<CartItem>();

            // Act
            var result = Order.Create(user, cartItems);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.EMPTY_CART_ITEMS, result.Message);
            Assert.Null(result.Data);
        }
    }
}
