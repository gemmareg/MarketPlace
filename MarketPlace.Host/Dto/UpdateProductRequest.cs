using static MarketPlace.Shared.Enums;

namespace MarketPlace.Host.Dto
{
    public class UpdateProductRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductState State { get; set; }
    }
}
