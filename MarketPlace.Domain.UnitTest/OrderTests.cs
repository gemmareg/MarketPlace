using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain.UnitTest
{
    public class OrderTests
    {
        private const string VALID_USERID = "fe88f2d6-e7a1-4d2e-ae88-d70c32917976";
        public const string VALID_USER_NAME = "Ramon";

        [Fact]
        public void Create_ShouldReturnOkResult_WhenParametersAreValid()
        {
            // Arrange
            var user = User.Create(new Guid(VALID_USERID), VALID_USER_NAME).Data!;
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
            var user = User.Create(new Guid(VALID_USERID), VALID_USER_NAME).Data!;
            var cartItems = new List<CartItem>();

            // Act
            var result = Order.Create(user, cartItems);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.EMPTY_CART_ITEMS, result.Message);
            Assert.Null(result.Data);
        }

        [Theory]
        [InlineData(OrderStatus.Pending, true)]
        [InlineData(OrderStatus.Paid, false)]
        [InlineData(OrderStatus.Sent, false)]
        [InlineData(OrderStatus.Delivered, false)]
        [InlineData(OrderStatus.Cancelled, false)]
        public void MarkAsPaid_Should_Only_Work_From_Pending(OrderStatus initialStatus, bool shouldSucceed)
        {
            var order = CreateValidOrder();

            typeof(Order)
                .GetProperty(nameof(Order.Status))!
                .SetValue(order, initialStatus);

            var result = order.MarkAsPaid(PaymentMethod.DebitCard);

            Assert.Equal(shouldSucceed, result.Success);
        }

        [Theory]
        [InlineData(OrderStatus.Paid, true)]
        [InlineData(OrderStatus.Pending, false)]
        [InlineData(OrderStatus.Sent, false)]
        [InlineData(OrderStatus.Delivered, false)]
        [InlineData(OrderStatus.Cancelled, false)]
        public void Ship_Should_Only_Work_From_Paid(OrderStatus initialStatus, bool shouldSucceed)
        {
            var order = CreateValidOrder();

            typeof(Order)
                .GetProperty(nameof(Order.Status))!
                .SetValue(order, initialStatus);

            var result = order.Send();

            Assert.Equal(shouldSucceed, result.Success);
        }

        [Theory]
        [InlineData(OrderStatus.Sent, true)]
        [InlineData(OrderStatus.Pending, false)]
        [InlineData(OrderStatus.Paid, false)]
        [InlineData(OrderStatus.Delivered, false)]
        [InlineData(OrderStatus.Cancelled, false)]
        public void Deliver_Should_Only_Work_From_Shipped(OrderStatus initialStatus, bool shouldSucceed)
        {
            var order = CreateValidOrder();

            typeof(Order)
                .GetProperty(nameof(Order.Status))!
                .SetValue(order, initialStatus);

            var result = order.Deliver();

            Assert.Equal(shouldSucceed, result.Success);
        }

        [Theory]
        [InlineData(OrderStatus.Pending, true)]
        [InlineData(OrderStatus.Paid, true)]
        [InlineData(OrderStatus.Sent, false)]
        [InlineData(OrderStatus.Delivered, false)]
        [InlineData(OrderStatus.Cancelled, false)]
        public void Cancel_Should_Not_Work_After_Shipping(OrderStatus initialStatus, bool shouldSucceed)
        {
            var order = CreateValidOrder();

            typeof(Order)
                .GetProperty(nameof(Order.Status))!
                .SetValue(order, initialStatus);

            var result = order.Cancel();

            Assert.Equal(shouldSucceed, result.Success);
        }

        #region private methods
        private static Order CreateValidOrder()
        {
            var user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            var category = Category.Create("Electronics", "Desc").Data!;
            var product = Product.Create(
                category,
                user,
                "Product",
                "Desc",
                10m,
                10,
                DateTime.UtcNow,
                Product.ProductState.Active).Data!;

            var cartItem = CartItem.Create(user.Id, product.Id, 2).Data!;
            var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

            return order;
        }
        #endregion
    }
}
