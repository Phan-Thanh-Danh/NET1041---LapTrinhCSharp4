using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, 1000, ErrorMessage = "Stock must be between 0 and 1000.")]
        public int Stock { get; set; }
    }
}