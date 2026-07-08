using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain.UnitTest
{
    public class CartItemTests
    {
        private const string VALID_USERID = "fe88f2d6-e7a1-4d2e-ae88-d70c32917976";
        private const string VALID_PRODUCTID = "fde2b44f-9ceb-4f3b-bbe4-3dedc9edd849";
        private const string EMPTY_GUID = "00000000-0000-0000-0000-000000000000";
        private const int VALID_QUANTITY = 3;

        [Theory]
        [InlineData(EMPTY_GUID, VALID_PRODUCTID, VALID_QUANTITY, false, ErrorMessages.INVALID_USER)]
        [InlineData(VALID_USERID, EMPTY_GUID, VALID_QUANTITY, false, ErrorMessages.INVALID_PRODUCT)]
        [InlineData(VALID_USERID, VALID_PRODUCTID, 0, false, ErrorMessages.INVALID_QUANTITY)]
        [InlineData(VALID_USERID, VALID_PRODUCTID, VALID_QUANTITY, true, "")]
        public void Create_ShouldReturnExpectedResult(string userId, string productId, int quantity, bool expectedSuccess, string expectedMessage)
        {
            // Arrange
            var user = WithId(User.Create(Guid.NewGuid(), "Test User").Data!, new Guid(userId));
            var category = Category.Create("Electronics", "Desc").Data!;
            var product = WithId(Product.Create(category, user, "Product", "Desc", 10m, 10, DateTime.UtcNow, ProductState.Active).Data!, new Guid(productId));

            // Act
            var result = CartItem.Create(user, product, quantity);

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedMessage, result.Message);

            if (expectedSuccess)
            {
                Assert.NotNull(result.Data);
                Assert.Equal(userId, result.Data.UserId.ToString());
                Assert.Equal(productId, result.Data.ProductId.ToString());
                Assert.Equal(quantity, result.Data.Quantity);
            }
            else
            {
                Assert.Null(result.Data);
            }
        }

        private static T WithId<T>(T entity, Guid id) where T : BaseEntity
        {
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(entity, id);
            return entity;
        }
    }
}
