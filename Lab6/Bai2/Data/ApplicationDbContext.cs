using Bai2.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
