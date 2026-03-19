namespace MarketPlace.Host.Dto
{
    public class UpdateCartItemRequest
    {
        public string CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
