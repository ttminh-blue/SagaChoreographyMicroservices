using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext db) : base(db)
        {
        }
    }
}
