using System;

namespace Lab2ModelBinding.Models
{
    public class OrderFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
    }
}
