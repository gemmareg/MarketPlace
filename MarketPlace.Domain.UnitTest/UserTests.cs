using MarketPlace.Shared;

namespace MarketPlace.Domain.UnitTest
{
    public class UserTests
    {
        private const string USERNAME = "testuser";
        private const string Id = "ef5d1b6e-ab0f-4b64-8f94-eb92e584c9a8";
        private const string EMPTY_GUID = "00000000-0000-0000-0000-000000000000";

        [Theory]
        [InlineData(USERNAME, Id, true, "")]
        [InlineData("  ", Id, false, ErrorMessages.INVALID_USER_NAME)]
        [InlineData(USERNAME, EMPTY_GUID, false, ErrorMessages.INVALID_USER_ID)]
        public void Create_ShouldReturnExpectedResult(string name, string id, bool expectedSuccess, string expectedMessage)
        {
            // Act
            var result = User.Create(new Guid(id), name);
            // Assert
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedMessage, result.Message);
            if (expectedSuccess)
            {
                Assert.NotNull(result.Data);
                Assert.Equal(name, result.Data!.Name);
                Assert.Equal(id, result.Data!.Id.ToString());
            }
            else
            {
                Assert.Null(result.Data);
            }
        }
    }
}
