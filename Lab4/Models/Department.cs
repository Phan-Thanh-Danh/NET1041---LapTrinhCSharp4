using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // Navigation property
    public virtual ICollection<Employee> Employees { get; set; }
}
}