using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai2.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên môn học")]
        public string? Title { get; set; }

        public int StudentId { get; set; }

        // Navigation property defined as virtual for Lazy Loading
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }
    }
}
