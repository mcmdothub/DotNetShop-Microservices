using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.OrderService.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetFlix.OrderService.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext()
        {
            // ...
        }

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
            // ...
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Set integrity on db columns

            builder.Entity<Order>(entity =>
            {
                entity.Property(x => x.OrderDate).IsRequired();
                entity.Property(x => x.OrderTotal).IsRequired().HasColumnType("decimal(18,2)");
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.Property(x => x.OrderId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Photo).HasMaxLength(200);
                entity.Property(x => x.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            });

            builder.Entity<Status>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(30);
            });

            builder.Entity<Status>().HasData(
                new Status() { Id = 1, Name = "Accepted" },
                new Status() { Id = 2, Name = "Processing" },
                new Status() { Id = 3, Name = "Shipped" },
                new Status() { Id = 4, Name = "Delivered" },
                new Status() { Id = 5, Name = "Completed" }
            );

        }
    }
}
