using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class UserRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly User _user;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            _userRepository = new UserRepository(_fixture.DbContext);
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_User()
        {
            // Act - Insert
            await _userRepository.AddAsync(_user);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Insert
            var insertedUser = await _userRepository.GetByIdAsync(_user.Id);
            insertedUser.Should().NotBeNull();
            insertedUser!.Name.Should().Be("John Doe");

            // Act - Read
            var allUsers = await _userRepository.GetAllAsync();

            // Assert - Read
            allUsers.Should().ContainSingle(u => u.Id == _user.Id);
             
            // Act - Delete
            await _userRepository.RemoveAsync(_user);
            await _fixture.DbContext.SaveChangesAsync();

            // Assert - Delete
            var deletedUser = await _userRepository.GetByIdAsync(_user.Id);
            deletedUser.Should().BeNull();
        }
    }
}
