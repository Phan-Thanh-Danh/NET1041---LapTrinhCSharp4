using Microsoft.EntityFrameworkCore;

namespace Bai3.Data
{
    public class AppdbContext : DbContext
    {
        public AppdbContext(DbContextOptions<AppdbContext> options) 
            : base(options)
        {
        }

        public DbSet<Models.Author> Authors { get; set; }
        public DbSet<Models.Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng Authors
            modelBuilder.Entity<Models.Author>(entity =>
            {
                entity.ToTable("Authors");
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Cấu hình bảng Books
            modelBuilder.Entity<Models.Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.BookTitle)
                    .IsRequired()
                    .HasMaxLength(200);

                // Cấu hình relationship
                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data (optional)
            modelBuilder.Entity<Models.Author>().HasData(
                new Models.Author { AuthorId = 1, AuthorName = "Nguyễn Nhật Ánh" },
                new Models.Author { AuthorId = 2, AuthorName = "Nguyễn Du" },
                new Models.Author { AuthorId = 3, AuthorName = "Nam Cao" }
            );

            modelBuilder.Entity<Models.Book>().HasData(
                new Models.Book { BookId = 1, BookTitle = "Mắt Biếc", PublicationYear = 1990, AuthorId = 1 },
                new Models.Book { BookId = 2, BookTitle = "Truyện Kiều", PublicationYear = 1820, AuthorId = 2 },
                new Models.Book { BookId = 3, BookTitle = "Chí Phèo", PublicationYear = 1941, AuthorId = 3 }
            );
        }
    }
}