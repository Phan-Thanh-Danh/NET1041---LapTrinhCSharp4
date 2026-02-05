using Bai2.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ 1-n giữa Student và Course
            modelBuilder
                .Entity<Course>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.StudentId);

            // Dữ liệu mẫu (Seed Data)
            modelBuilder
                .Entity<Student>()
                .HasData(
                    new Student { StudentId = 1, Name = "Phan Thanh Danh" },
                    new Student { StudentId = 2, Name = "Nguyễn Văn A" },
                    new Student { StudentId = 3, Name = "Trần Thị B" }
                );

            modelBuilder
                .Entity<Course>()
                .HasData(
                    new Course
                    {
                        CourseId = 1,
                        Title = "Lập trình C# 4",
                        StudentId = 1,
                    },
                    new Course
                    {
                        CourseId = 2,
                        Title = "ASP.NET Nâng cao",
                        StudentId = 1,
                    },
                    new Course
                    {
                        CourseId = 3,
                        Title = "Cơ sở dữ liệu",
                        StudentId = 2,
                    },
                    new Course
                    {
                        CourseId = 4,
                        Title = "Lập trình Web",
                        StudentId = 3,
                    }
                );
        }
    }
}
