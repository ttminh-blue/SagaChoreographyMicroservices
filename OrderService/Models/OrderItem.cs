namespace OrderService.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Units { get; set; }
        public Guid? OrderId { get; set; }
    }
}
