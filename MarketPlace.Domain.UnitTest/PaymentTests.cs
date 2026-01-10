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
        private readonly Category _category;
        private readonly Product _product;
        private readonly List<CartItem> _cartItems;

        public PaymentTests()
        {
            var userResult = User.Create(new Guid(VALID_USERID), VALID_USER_NAME);
            _user = userResult.Data!;
            _category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            _product = Product.Create(
                category: _category,
                seller: _user,
                name: "Test Product",
                description: "A product for testing",
                price: 9.99m,
                stock: 100,
                dateCreated: DateTime.UtcNow,
                state: ProductState.Active).Data!;
            _cartItems = new List<CartItem>(){
                CartItem.Create(_user.Id, _product.Id, 2).Data!,
            };
            _order = Order.Create(_user, _cartItems).Data!;
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult()
        {
            // Act
            var result = Order.Create(_user, _cartItems);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data!.Payment);
        }
    }
}
