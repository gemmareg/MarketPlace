using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.IntegrationTest.Connection;

public class EFConnectionTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly User _user;

    public EFConnectionTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _user = User.Create(Guid.NewGuid(), "John Doe").Data!;

    }

    [Fact] 
    public async Task Should_Connect_To_Database() 
    { 
        var canConnect = await _fixture.DbContext.Database.CanConnectAsync();
        Assert.True(canConnect); 
    }

    [Fact]
    public async Task Should_Insert_And_Read_User()
    {
        // Act
        _fixture.DbContext.Users.Add(_user);
        await _fixture.DbContext.SaveChangesAsync();

        // Assert
        var usersCount = await _fixture.DbContext.Users.CountAsync();
        Assert.Equal(1, usersCount);
    }

    [Fact] 
    public async Task Should_Insert_And_Read_Product() 
    {
        // Arrange
        var categoryName = "Electrónica";
        var categoryDescription = "Dispositivos y gadgets electrónicos";
        var product = Product.Create(
            category: Category.Create(categoryName, categoryDescription).Data!, 
            seller: _user, 
            name: "Test Product", 
            description: "A product for testing", 
            price: 9.99m, 
            stock: 100, 
            dateCreated: DateTime.UtcNow, 
            state: Product.ProductState.Active).Data!;

        // Act
        _fixture.DbContext.Products.Add(product); 
        await _fixture.DbContext.SaveChangesAsync();

        // Assert
        var productsCount = await _fixture.DbContext.Products.CountAsync();
        Assert.Equal(1, productsCount);
    }
}
