using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain.UnitTest
{
    public class PaymentTests
    {
        private const string VALID_USERID = "fe88f2d6-e7a1-4d2e-ae88-d70c32917976";
        public const string VALID_USER_NAME = "Ramon";
        private readonly User _user;
        private readonly Order _order;
        private readonly List<CartItem> _cartItems;

        public PaymentTests()
        {
            var userResult = User.Create(new Guid(VALID_USERID), VALID_USER_NAME);
            _user = userResult.Data!;
            _cartItems = new List<CartItem>(){
                CartItem.Create(_user.Id, Guid.NewGuid(), 2).Data!,
                CartItem.Create(_user.Id, Guid.NewGuid(), 1).Data!
            };
            _order = Order.Create(_user, _cartItems).Data!;
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult()
        {
            // Arrange
            decimal amount = 10.0m;
            PaymentMethods method = PaymentMethods.CreditCard;

            // Act
            var result = Payment.Create(null, amount, method);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal(ErrorMessages.INVALID_ORDER_FOR_PAYMENT, result.Message);
        }

        [Theory]
        [InlineData(10.0, PaymentMethods.CreditCard, true, "")]
        [InlineData(-5.0, PaymentMethods.DebitCard, false, ErrorMessages.INVALID_PAYMENT_AMOUNT)]
        [InlineData(15.0, (PaymentMethods)999, false, ErrorMessages.INVALID_PAYMENT_METHOD)]
        public void Create_ShouldReturnExpectedResult(decimal amount, PaymentMethods method, bool expectedResult, string expectedMessage)
        {
            // Act
            var result = Payment.Create(_order, amount, method);

            // Assert
            if (!expectedResult)
            {
                Assert.False(result.Success);
                Assert.Equal(expectedMessage, result.Message);
                Assert.Null(result.Data);
                return;
            }
            else
            {
                Assert.True(result.Success);
                Assert.NotNull(result.Data);
                Assert.Equal(_order.Id, result.Data!.OrderId);
                Assert.Equal(amount, result.Data.Amount);
                Assert.Equal(method, result.Data.PaymentMethod);
            }
        }
    }
}
