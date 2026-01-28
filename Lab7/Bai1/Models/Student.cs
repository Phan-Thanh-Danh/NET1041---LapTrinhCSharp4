using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bai1.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR(50)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        [Column(TypeName = "NVARCHAR(50)")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column(TypeName = "VARCHAR(100)")]
        public string? Email { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}