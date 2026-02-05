using System.ComponentModel.DataAnnotations;

namespace Bai2.Models
{
      public class Student
      {
            [Key]
            public int StudentId { get; set; }

            [Required]
            [StringLength(100)]
            [Display(Name = "Họ và tên")]
            public string? Name { get; set; }

            // Navigation property defined as virtual for Lazy Loading
            public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
      }
}
