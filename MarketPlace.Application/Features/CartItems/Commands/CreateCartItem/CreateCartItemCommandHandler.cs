using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartItemCommandHandler(IUserRepository userRepository, IProductRepository productRepository,ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new Guid(request.UserId));
            if (user == null) return Result.Fail("User not found.");

            var product = await _productRepository.GetByIdAsync(new Guid(request.ProductId));
            if (product == null) return Result.Fail("Product not found.");

            var cartItemResult = CartItem.Create(user.Id, product.Id, request.Quantity);
            if (!cartItemResult.Success) return Result.Fail(cartItemResult.Message);

            await _cartItemRepository.AddAsync(cartItemResult.Data!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
