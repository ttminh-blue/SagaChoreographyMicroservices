﻿using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
