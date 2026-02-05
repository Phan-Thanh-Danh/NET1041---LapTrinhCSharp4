using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai3.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        // Navigation property
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
