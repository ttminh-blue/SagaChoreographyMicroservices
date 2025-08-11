using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Units { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Orders Order { get; set; }
    }
}
