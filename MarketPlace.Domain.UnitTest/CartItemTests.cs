namespace MarketPlace.Domain.UnitTest
{
    public class CartItemTests
    {
        private const int VALID_USERID = 55;
        private const int VALID_PRODUCTID = 110;
        private const int VALID_QUANTITY = 3;

        [Theory]
        // Casos inválidos
        [InlineData(0, VALID_PRODUCTID, VALID_QUANTITY, false, "El usuario no es válido")]      // userId inválido
        [InlineData(VALID_USERID, 0, VALID_QUANTITY, false, "El producto no es válido")]     // productId inválido
        [InlineData(VALID_USERID, VALID_PRODUCTID, 0, false, "La cantidad no es válida")]     // quantity inválida
                                                                     // Caso válido
        [InlineData(1, 1, 5, true, "")]                               // todos válidos
        public void Create_ShouldReturnExpectedResult(int userId, int productId, int quantity, bool expectedSuccess, string expectedMessage)
        {

            // Act
            var result = CartItem.Create(userId, productId, quantity);

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedMessage, result.Message);

            if (expectedSuccess)
            {
                Assert.NotNull(result.Data);
                Assert.Equal(userId, result.Data.UserId);
                Assert.Equal(productId, result.Data.ProductId);
                Assert.Equal(quantity, result.Data.Quantity);
            }
            else
            {
                Assert.Null(result.Data);
            }
        }
    }
}
