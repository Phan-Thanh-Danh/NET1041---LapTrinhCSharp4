using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bai3.Models;

namespace Bai3.Views
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề sách là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề sách")]
        public string BookTitle { get; set; } = string.Empty;

        [Range(1000, 9999, ErrorMessage = "Năm xuất bản phải từ 1000 đến 9999")]
        [Display(Name = "Năm xuất bản")]
        public int? PublicationYear { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tác giả")]
        [Display(Name = "Tác giả")]
        public int AuthorId { get; set; }

        // Navigation property
        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}