using Microsoft.EntityFrameworkCore;
using Bai4.Models;

namespace Bai4.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Tự động tạo database nếu chưa có
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Nguyễn Văn A" },
                new Student { StudentId = 2, Name = "Trần Thị B" },
                new Student { StudentId = 3, Name = "Lê Văn C" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, Title = "Lập trình C#" },
                new Course { CourseId = 2, Title = "Cơ sở dữ liệu" },
                new Course { CourseId = 3, Title = "Web nâng cao" }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { EnrollmentId = 1, StudentId = 1, CourseId = 1 },
                new Enrollment { EnrollmentId = 2, StudentId = 1, CourseId = 2 },
                new Enrollment { EnrollmentId = 3, StudentId = 2, CourseId = 2 },
                new Enrollment { EnrollmentId = 4, StudentId = 3, CourseId = 1 },
                new Enrollment { EnrollmentId = 5, StudentId = 3, CourseId = 3 }
            );
        }
    }
}
