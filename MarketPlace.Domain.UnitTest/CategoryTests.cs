using MarketPlace.Shared;

namespace MarketPlace.Domain.UnitTest
{
    public class CategoryTests
    {
        public const string VALID_NAME = "Electrónica";
        public const string VALID_DESCRIPTION = "Dispositivos y gadgets electrónicos";

        [Theory]
        [InlineData(VALID_NAME, VALID_DESCRIPTION, true)]
        [InlineData(VALID_NAME, null, true)]
        [InlineData("", "", false)]
        [InlineData(null, VALID_DESCRIPTION, false)]
        public void Create_ValidParameters_ReturnsSuccess(string nombre, string? descripcion, bool expectedSuccess)
        {
            // Act
            var result = Category.Create(nombre, descripcion);
            // Assert
            Assert.Equal(result.Success, expectedSuccess);

            if (expectedSuccess){
                Assert.NotNull(result.Data);
                Assert.Equal(nombre, result.Data!.Name);
                Assert.Equal(descripcion, result.Data.Description);
            }
            else
            {
                Assert.Equal(ErrorMessages.INVALID_CATEGORY_NAME, result.Message);
            }
        }
    }
}
