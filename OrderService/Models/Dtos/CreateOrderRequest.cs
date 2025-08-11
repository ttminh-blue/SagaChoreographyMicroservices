namespace OrderService.Models.Dtos
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public List<OrderDTO> OrderItems { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Guid OrderID { get; set; }
    }
}
