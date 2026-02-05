using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bai4.Models
{
      public class Student
      {
            public int StudentId { get; set; }

            [Display(Name = "Tên học sinh")]
            public string Name { get; set; } = null!;

            // Navigation property for Lazy Loading (must be virtual)
            public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
      }
}
