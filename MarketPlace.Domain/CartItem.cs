using MarketPlace.Shared;

namespace MarketPlace.Domain
{
    public class CartItem
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }        
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public User? User { get; private set; }
        public Product? Product { get; private set; }

        private CartItem() { }

        public static Result<CartItem> Create(int userId, int productId, int quantity)
        {
            if (userId == 0) 
            {
                return Result<CartItem>.Fail("El usuario no es válido");
            }

            if (productId == 0) 
            {
                return Result<CartItem>.Fail("El producto no es válido");
            }

            if(quantity == 0)
            {
                return Result<CartItem>.Fail("La cantidad no es válida");
            }

            var cartItem = new CartItem();
            cartItem.UserId = userId;
            cartItem.ProductId = productId;
            cartItem.Quantity = quantity;

            return Result<CartItem>.Ok(cartItem);
        }

    }
}
