namespace MarketPlace.Application.Dtos
{
    public class OrderResumedDto
    {
        public Guid OrderId { get; set; }
        public int ItemsCount { get; set; }
        public decimal Total { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
