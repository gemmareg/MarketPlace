namespace MarketPlace.Application.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
