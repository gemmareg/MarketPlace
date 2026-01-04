using FluentAssertions;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.IntegrationTest.Fixtures;
using MarketPlace.Infrastructure.Persistance.Repositories;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Infrastructure.IntegrationTest.Repositories
{
    [Collection("Database collection")]
    public class OrderRepositoryTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly IOrderRepository _orderRepository;

        private readonly User _user;
        private readonly Category _category;
        private readonly Product _product;
        private readonly CartItem _cartItem;
        private readonly Order _order;

        private const int QUANTITY = 2;

        public OrderRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _orderRepository = new OrderRepository(_fixture.DbContext);
            _category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
            _user = User.Create(Guid.NewGuid(), "John Doe").Data!;
            _product = Product.Create(
                category: _category,
                seller: _user,
                name: "Test Product",
                description: "A product for testing",
                price: 9.99m,
                stock: 100,
                dateCreated: DateTime.UtcNow,
                state: Product.ProductState.Active).Data!;

            _fixture.DbContext.Categories.Add(_category);
            _fixture.DbContext.Users.Add(_user);
            _fixture.DbContext.Products.Add(_product);
            _fixture.DbContext.SaveChanges();

            _cartItem = CartItem.Create(
                userId: _user.Id,
                productId: _product.Id,
                quantity: QUANTITY).Data!;
            _order = Order.Create(
                user: _user,
                cartItems: new List<CartItem> { _cartItem }).Data!;
        }

        [Fact]
        public async Task Should_Insert_Read_And_Delete_Payment()
        {
            // Act - Insert
            await _orderRepository.AddAsync(_order);
            _fixture.DbContext.SaveChanges();

            // Assert - Insert
            var insertedOrder = await _orderRepository.GetByIdAsync(_order.Id);
            insertedOrder.Should().NotBeNull();

            // Act - Read
            var allOrders = await _orderRepository.GetAllAsync();

            // Assert - Read
            allOrders.Should().ContainSingle(u => u.Id == _order.Id);

            // Arrange Update
            insertedOrder.MarkAsPaid(PaymentMethod.CreditCard);

            // Act - Update
            await _orderRepository.UpdateAsync(insertedOrder);
            _fixture.DbContext.SaveChanges();

            // Assert - Update
            var updatedOrder = await _orderRepository.GetByIdAsync(_order.Id);
            updatedOrder!.Status.Should().Be(OrderStatus.Paid);

            // Act - Delete
            await _orderRepository.RemoveAsync(updatedOrder);
            _fixture.DbContext.SaveChanges();

            // Assert - Delete
            var deletedOrder = await _orderRepository.GetByIdAsync(_order.Id);
            deletedOrder.Should().BeNull();
        }
    }
}