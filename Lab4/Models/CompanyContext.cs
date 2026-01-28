using Lab4.Models;
using Microsoft.EntityFrameworkCore;

public class CompanyContext : DbContext
{
    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options)
    {
    }

    public DbSet<Information> Informations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
}