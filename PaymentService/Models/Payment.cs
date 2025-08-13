namespace PaymentService.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public string TransactionId { get; set; }
    }
}
