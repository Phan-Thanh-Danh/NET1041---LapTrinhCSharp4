using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bai3.Models
{
      public class Category
      {
            [Key]
            public int CategoryId { get; set; }

            [Required]
            [Display(Name = "Tên danh mục")]
            public string CategoryName { get; set; } = string.Empty;

            // Navigation property
            public virtual ICollection<Product> Products { get; set; } = new List<Product>();
      }
}
