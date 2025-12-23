using MarketPlace.Shared;

namespace MarketPlace.Domain.UnitTest
{
    public class OrderItemTests
    {
        private CartItem _cartItem;
        private Order _order;
        private User _user;

        private const string VALID_USERID = "fe88f2d6-e7a1-4d2e-ae88-d70c32917976";
        private const string VALID_PRODUCTID = "fde2b44f-9ceb-4f3b-bbe4-3dedc9edd849";
        private const int VALID_QUANTITY = 3;
        private const string USERNAME = "testuser";
        private const string USEREMAIL = "testuser@test.com";
        private const string PASSWORD = "hashedpassword";

        public OrderItemTests()
        {
            _user = User.Create(USERNAME, USEREMAIL, PASSWORD).Data!;
            _cartItem = CartItem.Create(new Guid(VALID_USERID), new Guid(VALID_PRODUCTID), VALID_QUANTITY).Data!;
            _order = Order.Create(_user, [_cartItem]).Data!;
        }

        [Fact]
        public void Create_ShouldReturnFailResult_WhenOrderIsNull()
        {
            // Act
            var result = OrderItem.Create(_cartItem, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.INVALID_ORDER_FOR_ORDER_ITEM, result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public void Create_ShouldReturnFailResult_WhenCartItemIsNull()
        {
            // Act
            var result = OrderItem.Create(null!, _order);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.INVALID_CART_ITEM_FOR_ORDER_ITEM, result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public void Create_ShouldReturnOkResult_WhenParametersAreValid()
        {
            // Act
            var result = OrderItem.Create(_cartItem, _order);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(_order.Id, result.Data!.OrderId);
            Assert.Equal(_cartItem.ProductId, result.Data.ProductId);
            Assert.Equal(_cartItem.Quantity, result.Data.Quantity);
            Assert.Equal(_cartItem.Product?.Price ?? 0m, result.Data.UnitPrice);
        }
    }
}
