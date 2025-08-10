using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OrderService.Models.Context
{
    public class OrderDb : DbContext
    {
        public OrderDb(DbContextOptions<OrderDb> options) : base(options)
        { }

        public DbSet<Orders> Orders => Set<Orders>();
    } 
}
