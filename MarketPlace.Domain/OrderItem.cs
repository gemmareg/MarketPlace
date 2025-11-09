namespace MarketPlace.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Order? Pedido { get; set; }

        public int ProductoId { get; set; }
        public Product? Producto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
