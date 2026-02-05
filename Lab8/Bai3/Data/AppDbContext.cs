using Bai3.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Điện thoại" },
                new Category { CategoryId = 2, CategoryName = "Laptop" },
                new Category { CategoryId = 3, CategoryName = "Phụ kiện" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "iPhone 15 Pro Max", CategoryId = 1 },
                new Product { ProductId = 2, Name = "Samsung Galaxy S24 Ultra", CategoryId = 1 },
                new Product { ProductId = 3, Name = "MacBook Pro M3", CategoryId = 2 },
                new Product { ProductId = 4, Name = "Dell XPS 13", CategoryId = 2 },
                new Product { ProductId = 5, Name = "AirPods Pro 2", CategoryId = 3 }
            );
        }
    }
}
