using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
public class Employee
{
    public int Id { get; set; }
    public int Departmentid { get; set; } // Khóa ngoại (FK)
    public string? Name { get; set; }
    public string? Designation { get; set; }

    // Navigation property
    public virtual Department Department { get; set; }
}
}