using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai3.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Tên tác giả là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự")]
        [Display(Name = "Tên tác giả")]
        public string AuthorName { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}