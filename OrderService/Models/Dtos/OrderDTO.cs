namespace OrderService.Models.Dtos
{
    public class OrderDTO
    {
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Units { get; set; }
    }
}
