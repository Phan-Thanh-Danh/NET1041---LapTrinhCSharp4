using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bai4.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        
        [Display(Name = "Tên môn học")]
        public string Title { get; set; } = null!;

        // Navigation property for Lazy Loading (must be virtual)
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
