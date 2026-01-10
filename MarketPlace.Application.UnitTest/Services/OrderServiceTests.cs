using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Services;
using MarketPlace.Domain;
using Moq;
using static MarketPlace.Domain.Product;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.UnitTest.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<ICartItemRepository> _cartRepo = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private const string USER_ID = "0a046732-ab9a-41bf-9ba9-c2f242baf7a2";
    private const string CARTITEM_ID = "90895126-b6e0-4722-8a28-344e5ec5fbc0";
    private const string USER_NAME = "testuser";

    private readonly IOrderService _orderService;

    public OrderServiceTests()
    {
        _orderService = new OrderService(_orderRepo.Object, _cartRepo.Object, _userRepo.Object, _productRepo.Object, _uow.Object, _mapper.Object);
    }

    #region CreateOrder Tests
    [Fact]
    public async Task CreateOrder_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((User?)null);

        // Act
        var result = await _orderService.CreateOrder(USER_ID, new List<string>());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("User doesn't exist", result.Message);
    }

    [Fact]
    public async Task CreateOrder_ShouldFail_WhenCartItemsEmpty()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(user);

        _cartRepo.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()))
                 .ReturnsAsync(new List<CartItem>());

        // Act
        var result = await _orderService.CreateOrder(USER_ID, new List<string> { CARTITEM_ID });

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No items found", result.Message);
    }

    [Fact]
    public async Task CreateOrder_ShouldFail_WhenStockInsufficient()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 1, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 2).Data!;

        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(user);

        _cartRepo.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()))
                 .ReturnsAsync(new List<CartItem> { cartItem });

        _productRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(product);

        _productRepo.Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Product> { product });

        // Act
        var result = await _orderService.CreateOrder(USER_ID, new List<string> { CARTITEM_ID });

        // Assert
        Assert.False(result.Success);
        Assert.Contains($"Insufficient stock for product {product.Name}", result.Message);
    }
    #endregion

    #region CancelOrder Tests
    [Fact]
    public async Task CreateOrder_ShouldSucceed_AndPersistOrder()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 2).Data!;

        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(user);

        _cartRepo.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()))
                 .ReturnsAsync(new List<CartItem> { cartItem });

        _productRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(product);

        _productRepo.Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Product> { product });

        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

        // Act
        var result = await _orderService.CreateOrder(USER_ID, new List<string> { CARTITEM_ID });

        // Assert
        Assert.True(result.Success);

        _productRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        _orderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CancelOrder_ShouldSucceed_AndRestoreStock()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 2).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(order);

        _productRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(product);

        // Act
        var result = await _orderService.CancelOrder(order.Id.ToString());

        // Assert
        Assert.True(result.Success);
        _productRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        _orderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CancelOrder_ShouldFail_WhenOrderNotFound()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Order?)null);
        // Act
        var result = await _orderService.CancelOrder(Guid.NewGuid().ToString());
        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found", result.Message);
    }
    #endregion

    #region MarkOrderAsPaid Tests
    [Fact]
    public async Task MarkOrderAsPaid_ShouldSucceed_WhenOrderExists()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 1).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(order);

        // Act
        var result = await _orderService.MarkOrderAsPaid(order.Id.ToString(),1);

        // Assert
        Assert.True(result.Success);
        _orderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task MarkOrderAsPaid_ShouldFail_WhenOrderNotFound()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.MarkOrderAsPaid(Guid.NewGuid().ToString(),1);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found", result.Message);
    }
    #endregion

    #region SendOrder Tests
    [Fact]
    public async Task SendOrder_ShouldSucceed_WhenOrderExists()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 1).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(order);

        // Act
        var result = await _orderService.SendOrder(order.Id.ToString());

        // Assert
        Assert.True(result.Success);
        _orderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SendOrder_ShouldFail_WhenOrderNotFound()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.SendOrder(Guid.NewGuid().ToString());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found", result.Message);
    }
    #endregion

    #region DeliverOrder Tests
    [Fact]
    public async Task DeliverOrder_ShouldSucceed_WhenOrderExists()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 1).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;

        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(order);

        // Act
        var result = await _orderService.DeliverOrder(order.Id.ToString());

        // Assert
        Assert.True(result.Success);
        _orderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeliverOrder_ShouldFail_WhenOrderNotFound()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.DeliverOrder(Guid.NewGuid().ToString());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found", result.Message);
    }
    #endregion

    #region GetOrderById Tests
    [Fact]
    public async Task GetOrderById_ShouldReturnOrder_WhenFound()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 1).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;
        var orderDto = new OrderDto() { Id = order.Id.ToString(), UserId = order.UserId.ToString(), OrderDate = order.CreatedDate, TotalAmount = order.Total, Status = order.Status.ToString() };

        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(order);

        _mapper.Setup(m => m.Map<OrderDto>(It.IsAny<Order>()))
               .Returns(orderDto);

        // Act
        var result = await _orderService.GetOrderById(order.Id.ToString());

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetOrderById_ShouldFail_WhenOrderNotFound()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.GetOrderById(Guid.NewGuid().ToString());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found", result.Message);
    }
    #endregion

    #region GetOrdersByUserId Tests
    [Fact]
    public async Task GetOrdersByUserId_ShouldReturnOrders_WhenFound()
    {
        // Arrange
        var user = User.Create(Guid.Parse(USER_ID), USER_NAME).Data!;
        var category = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        var product = Product.Create(category, user, "Test Product", "Test Description", 100, 10, DateTime.Now, ProductState.Active).Data!;
        var cartItem = CartItem.Create(user.Id, product.Id, 1).Data!;
        var order = Order.Create(user, new List<CartItem> { cartItem }).Data!;
        var orderDto = new OrderDto() { Id = order.Id.ToString(), UserId = order.UserId.ToString(), OrderDate = order.CreatedDate, TotalAmount = order.Total, Status = order.Status.ToString() };

        _orderRepo.Setup(r => r.GetOrdersByUserIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(new List<Order> { order });

        _mapper.Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<Order>>()))
               .Returns([orderDto]);

        // Act
        var result = await _orderService.GetOrdersByUserId(USER_ID);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task GetOrdersByUserId_ShouldReturnEmpty_WhenNoOrders()
    {
        // Arrange
        _orderRepo.Setup(r => r.GetOrdersByUserIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(new List<Order>());

        _mapper.Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<Order>>()))
               .Returns(new List<OrderDto>());

        // Act
        var result = await _orderService.GetOrdersByUserId(USER_ID);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }
    #endregion
}
