namespace OrderService.Models.Dtos
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public List<OrderDTO> OrderItems { get; set; }
    }
}
