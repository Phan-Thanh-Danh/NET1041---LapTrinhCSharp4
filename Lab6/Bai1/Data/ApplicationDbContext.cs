using Bai1.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Gender).IsRequired();

                entity.Property(e => e.BirthDate).IsRequired().HasColumnType("date");

                entity.Property(e => e.BatchTime).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Phone).IsRequired().HasMaxLength(10);

                entity.Property(e => e.WebUrl).HasMaxLength(255);

                entity.Property(e => e.Password).IsRequired().HasMaxLength(20);

                entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
            });
        }
    }
}
