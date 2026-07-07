using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.CartItems.Commands.CreateCartItem;
using MarketPlace.Application.Features.CartItems.Commands.DeleteCartItem;
using MarketPlace.Application.Features.CartItems.Commands.UpdateItem;
using MarketPlace.Application.Features.CartItems.Queries.GetCartItems;
using MarketPlace.Domain;
using Moq;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.UnitTest.Features
{
    public class CartItemTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IMapper> _mapper = new();

        private readonly CancellationToken _ct = CancellationToken.None;

        private readonly CreateCartItemCommandHandler _createCartItemCommandHandler;
        private readonly UpdateCartItemCommandHandler _updateCartItemCommandHandler;
        private readonly DeleteCartItemCommandHandler _deleteCartItemCommandHandler;
        private readonly GetCartItemsQueryHandler _getCartItemsQueryHandler;

        private readonly User _userFixture = User.Create(Guid.NewGuid(), "John").Data!;
        private readonly User _sellerFixture = User.Create(Guid.NewGuid(), "Jenny").Data!;
        private readonly Category _categoryFixture = Category.Create("Electronics", "Electronic devices and gadgets").Data!;
        private readonly Product _productFixture;


        public CartItemTests()
        {
            _createCartItemCommandHandler = new CreateCartItemCommandHandler(
                _userRepository.Object,
                _productRepository.Object,
                _cartItemRepository.Object,
                _unitOfWork.Object);
            _updateCartItemCommandHandler = new UpdateCartItemCommandHandler(
                _cartItemRepository.Object,
                _unitOfWork.Object);
            _deleteCartItemCommandHandler = new DeleteCartItemCommandHandler(
                _cartItemRepository.Object,
                _unitOfWork.Object);
            _getCartItemsQueryHandler = new GetCartItemsQueryHandler(_cartItemRepository.Object);

            _productFixture = Product.Create(_categoryFixture, _sellerFixture, "test product", "test description", 5, 10, DateTime.Now, ProductState.Active).Data!;
        }

        #region CreateCartItem Tests
        [Fact]
        public async Task CreateCartItem_Should_Return_Ok_When_Data_Is_Valid()
        {
            // Arrange
            _userRepository.Setup(r => r.GetByIdAsync(_userFixture.Id)).ReturnsAsync(_userFixture);
            _productRepository.Setup(r => r.GetByIdAsync(_productFixture.Id)).ReturnsAsync(_productFixture);

            var command = new CreateCartItemCommand
            {
                UserId = _userFixture.Id.ToString(),
                ProductId = _productFixture.Id.ToString(),
                Quantity = 2
            };

            // Act
            var result = await _createCartItemCommandHandler.Handle(command, _ct);

            // Assert
            Assert.True(result.Success);
            _cartItemRepository.Verify(r => r.AddAsync(It.IsAny<CartItem>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(_ct), Times.Once);
        }

        [Fact]
        public async Task CreateCartItem_Should_Fail_Ok_When_User_Buys_Them_Product()
        {
            // Arrange
            var ownProduct = Product.Create(_categoryFixture, _userFixture, "own product", "own product description", 5, 10, DateTime.Now, ProductState.Active).Data!;

            _userRepository.Setup(r => r.GetByIdAsync(_userFixture.Id)).ReturnsAsync(_userFixture);
            _productRepository.Setup(r => r.GetByIdAsync(ownProduct.Id)).ReturnsAsync(ownProduct);

            var command = new CreateCartItemCommand
            {
                UserId = _userFixture.Id.ToString(),
                ProductId = ownProduct.Id.ToString(),
                Quantity = 2
            };

            // Act
            var result = await _createCartItemCommandHandler.Handle(command, _ct);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("You cannot buy your own product", result.Message);
        }

        [Fact]
        public async Task CreateCartItem_Should_Fail_When_User_Not_Found()
        {
            // Arrange
            _userRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            var command = new CreateCartItemCommand
            {
                UserId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 2
            };

            // Act
            var result = await _createCartItemCommandHandler.Handle(command, _ct);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found.", result.Message);
        }
        #endregion

        #region UpdateCartItem Tests
        [Fact]
        public async Task UpdateCartItem_Should_Update_Quantity()
        {
            // Arrange
            var cartItem = CartItem.Create(_userFixture, _productFixture, 1).Data!;

            _cartItemRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cartItem);

            var command = new UpdateCartItemCommand
            {
                UserId = _userFixture.Id.ToString(),
                CartItemId = cartItem.Id.ToString(),
                Quantity = 5
            };

            // Act
            var result = await _updateCartItemCommandHandler.Handle(command,_ct);

            Assert.True(result.Success);
            Assert.Equal(5, cartItem.Quantity);
        }
        #endregion

        #region DeleteCartItem Tests
        [Fact]
        public async Task DeleteCartItem_Should_Remove_Item_When_Found()
        {
            // Arrange
            var cartItem = CartItem.Create(_userFixture, _productFixture, 1).Data!;

            _cartItemRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cartItem);

            var command = new DeleteCartItemCommand
            {
                UserId = _userFixture.Id.ToString(),
                CartItemId = cartItem.Id.ToString()
            };

            // Act
            var result = await _deleteCartItemCommandHandler.Handle(command,_ct);

            // Assert
            Assert.True(result.Success);
            _cartItemRepository.Verify(r => r.RemoveAsync(cartItem), Times.Once);
        }
        #endregion

        #region GetCartItems Tests
        [Fact]
        public async Task GetCartItems_Should_Return_List_When_Items_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                CartItem.Create(_sellerFixture, _productFixture, 1).Data!
            };

            _cartItemRepository.Setup(r => r.GetCartItemsByUserId(userId))
                .ReturnsAsync(cartItems);

            _mapper.Setup(m => m.Map<List<CartItemDto>>(cartItems))
                .Returns(new List<CartItemDto> { new CartItemDto() });

            var command = new GetCartItemsQuery
            {
                UserId = userId.ToString()
            };

            // Act
            var result = await _getCartItemsQueryHandler.Handle(command, _ct);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
        #endregion
    }
}
