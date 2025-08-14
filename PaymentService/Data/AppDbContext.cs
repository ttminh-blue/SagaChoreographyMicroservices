using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.OrderId)
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.Amount)
                      .HasPrecision(18, 2)
                      .IsRequired();

                entity.Property(e => e.PaymentMethod).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PaymentStatus).IsRequired();
                entity.Property(e => e.TransactionId).HasMaxLength(100);

            });
        }
    }
}
