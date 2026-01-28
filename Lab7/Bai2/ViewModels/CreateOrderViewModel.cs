using System.ComponentModel.DataAnnotations;
using Bai2.Models;

namespace OrderManagement.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Ngày đặt hàng là bắt buộc")]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự")]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Chi tiết đơn hàng")]
        public List<OrderDetailInput> OrderDetails { get; set; } = new List<OrderDetailInput>();
    }

    public class OrderDetailInput
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }
    }
}