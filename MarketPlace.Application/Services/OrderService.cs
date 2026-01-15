using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICartItemRepository cartItemRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateOrder(string userId, List<string> cartItemIds)
        {
            var user = await GetUserAsync(userId);
            if (user == null) return Result.Fail("User doesn't exist");

            var cartItems = await GetCartItemsAsync(cartItemIds);
            if (cartItems == null || cartItems.Count == 0) return Result.Fail("No items found");

            var stockCheck = await ValidateStock(cartItems);
            if (!stockCheck.Success) return stockCheck;

            var orderResult = Order.Create(user, cartItems);
            if (!orderResult.Success) return orderResult;

            await DeductStockAsync(cartItems);
            await _orderRepository.AddAsync(orderResult.Data!);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> CancelOrder(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(Guid.Parse(orderId));
            if (order == null) return Result.Fail("Order not found");      

            order.Cancel();
            await RestoreStockAsync(order.OrderItems);

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> MarkOrderAsPaid(string orderId, int paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(Guid.Parse(orderId));
            if (order == null) return Result.Fail("Order not found");
            
            order.MarkAsPaid((PaymentMethod)paymentMethod);

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> SendOrder(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(Guid.Parse(orderId));
            if (order == null) return Result.Fail("Order not found");
            order.Send();
            
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> DeliverOrder(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(Guid.Parse(orderId));
            if (order == null) return Result.Fail("Order not found");
            order.Deliver();
            
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<OrderDto>> GetOrderById(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(Guid.Parse(orderId));
            if (order == null) return Result<OrderDto>.Fail("Order not found");

            return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<List<OrderDto>>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(Guid.Parse(userId));
            return Result<List<OrderDto>>.Ok(_mapper.Map<List<OrderDto>>(orders));
        }

        #region private methods
        private async Task<User?> GetUserAsync(string userId)
            => await _userRepository.GetByIdAsync(Guid.Parse(userId));

        private async Task<List<CartItem>?> GetCartItemsAsync(List<string> ids)
            => await _cartItemRepository.GetByIdsAsync(ids.Select(Guid.Parse).ToList());

        private async Task<Result> ValidateStock(IEnumerable<CartItem> cartItems)
        {
            var products = await _productRepository.GetProductsByIdsAsync(cartItems.Select(c => c.ProductId).ToList());
            
            foreach (var item in cartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    return Result.Fail($"Product with ID {item.ProductId} not found");
                if (product.Stock < item.Quantity)
                    return Result.Fail($"Insufficient stock for product {product.Name}");
            }

            return Result.Ok();
        }

        private async Task DeductStockAsync(IEnumerable<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId); 
                product.DeductStock(item.Quantity); 
                
                await _productRepository.UpdateAsync(product);
            }
        }

        private async Task RestoreStockAsync(IEnumerable<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId); 
                product!.ReplenishStock(item.Quantity); 
                
                await _productRepository.UpdateAsync(product);
            }
        }
        #endregion
    }
}