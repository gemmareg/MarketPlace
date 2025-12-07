using MarketPlace.Shared;

namespace MarketPlace.Domain.UnitTest
{
    public class UserTests
    {
        private const string USERNAME = "testuser";
        private const string USEREMAIL = "testuser@test.com";
        private const string PASSWORD = "hashedpassword";

        [Theory]
        [InlineData(USERNAME, USEREMAIL, PASSWORD, true, "")]
        [InlineData("  ", USEREMAIL, PASSWORD, false, ErrorMessages.INVALID_USER_NAME)]
        [InlineData(USERNAME, "  ", PASSWORD, false, ErrorMessages.INVALID_USER_EMAIL)]
        [InlineData(USERNAME, USEREMAIL, "  ", false, ErrorMessages.INVALID_USER_PASSWORD)]
        public void Create_ShouldReturnExpectedResult(string name, string email, string passwordHash, bool expectedSuccess, string expectedMessage)
        {
            // Act
            var result = User.Create(name, email, passwordHash);
            // Assert
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedMessage, result.Message);
            if (expectedSuccess)
            {
                Assert.NotNull(result.Data);
                Assert.Equal(name, result.Data!.Name);
                Assert.Equal(email, result.Data!.Email);
                Assert.Equal(passwordHash, result.Data!.PasswordHash);
                Assert.Equal(User.UserRole.Buyer, result.Data!.Role);
            }
            else
            {
                Assert.Null(result.Data);
            }
        }
    }
}
