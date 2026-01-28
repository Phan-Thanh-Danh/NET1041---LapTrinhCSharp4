using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai1.Models
{
    [Table("students")]
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("first_name", TypeName = "varchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name", TypeName = "varchar(100)")]
        public string LastName { get; set; } = string.Empty;

        // Navigation (1–1)
        public Student_Detail? StudentDetail { get; set; }
    }

}