namespace MarketPlace.Host.Dto
{
    public class CreateCartItemRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
