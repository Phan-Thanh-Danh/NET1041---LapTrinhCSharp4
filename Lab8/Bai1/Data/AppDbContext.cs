using Microsoft.EntityFrameworkCore;
using Bai1.Models;
using System;

namespace Bai1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Dishes)
                .HasForeignKey(d => d.OrderId);

            // Seed Data
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Name = "John Doe" },
                new Customer { CustomerId = 2, Name = "Jane Smith" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, CustomerId = 1, OrderDate = new DateTime(2025, 2, 1) },
                new Order { OrderId = 2, CustomerId = 1, OrderDate = new DateTime(2025, 2, 2) },
                new Order { OrderId = 3, CustomerId = 2, OrderDate = new DateTime(2025, 2, 3) }
            );

            modelBuilder.Entity<Dish>().HasData(
                new Dish { DishId = 1, OrderId = 1, Name = "Pizza Margherita" },
                new Dish { DishId = 2, OrderId = 1, Name = "Spaghetti Carbonara" },
                new Dish { DishId = 3, OrderId = 2, Name = "Grilled Salmon" },
                new Dish { DishId = 4, OrderId = 3, Name = "Caesar Salad" },
                new Dish { DishId = 5, OrderId = 3, Name = "Beef Steak" }
            );
        }
    }
}
