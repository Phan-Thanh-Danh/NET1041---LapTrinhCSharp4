using Microsoft.EntityFrameworkCore;

namespace Lab2ModelBinding.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop Dell XPS 13", Description = "Laptop mỏng nhẹ cao cấp", Price = 25000000 },
                new Product { Id = 2, Name = "MacBook Air M2", Description = "Siêu phẩm Apple", Price = 28000000 },
                new Product { Id = 3, Name = "Chuột Logitech MX Master 3", Description = "Chuột công thái học", Price = 2500000 },
                new Product { Id = 4, Name = "Bàn phím Keychron K2", Description = "Bàn phím cơ không dây", Price = 1800000 },
                new Product { Id = 5, Name = "Màn hình LG UltraFine", Description = "Màn hình 4K sắc nét", Price = 12000000 }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, CustomerName = "Nguyễn Văn A", OrderDate = new DateTime(2025, 1, 1), Status = "Processing" },
                new Order { OrderId = 2, CustomerName = "Trần Thị B", OrderDate = new DateTime(2025, 1, 5), Status = "Completed" }
            );

            // Seed OrderDetails
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { OrderDetailId = 1, OrderId = 1, ProductName = "Laptop Dell XPS 13", Quantity = 1, Price = 25000000 },
                new OrderDetail { OrderDetailId = 2, OrderId = 1, ProductName = "Chuột Logitech MX Master 3", Quantity = 1, Price = 2500000 },
                new OrderDetail { OrderDetailId = 3, OrderId = 2, ProductName = "MacBook Air M2", Quantity = 1, Price = 28000000 }
            );
        }
    }
}
