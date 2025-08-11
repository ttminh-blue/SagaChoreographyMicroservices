using CommonModels;

namespace OrderService.Models
{
    public class Orders
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public double OrderAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
