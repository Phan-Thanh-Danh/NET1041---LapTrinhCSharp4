using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai1.Models
{
    [Table("student_details")]
    public class Student_Detail 
    {
        [Key]
        [ForeignKey(nameof(Student))]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; } = string.Empty;

        [Column("date_birth", TypeName = "date")]
        public DateTime DateBirth { get; set; }

        // Navigation
        public Student Student { get; set; } = null!;
    }
}