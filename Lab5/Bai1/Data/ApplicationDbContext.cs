using Microsoft.EntityFrameworkCore;
using Bai1.Models;

namespace Bai1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Student_Detail> StudentDetails { get; set; }
    }
}
