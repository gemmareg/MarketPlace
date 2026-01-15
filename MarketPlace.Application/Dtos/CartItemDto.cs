namespace MarketPlace.Application.Dtos
{
    public class CartItemDto
    {
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}
