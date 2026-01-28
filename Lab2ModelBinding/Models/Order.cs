using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab2ModelBinding.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
