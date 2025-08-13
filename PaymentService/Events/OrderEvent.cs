using PaymentService.Models;

namespace PaymentService.Events
{
    public class OrderEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
