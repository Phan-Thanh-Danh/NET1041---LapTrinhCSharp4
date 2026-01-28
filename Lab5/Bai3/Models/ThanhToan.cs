using System;
using System.ComponentModel.DataAnnotations;

namespace Bai3.Models
{
    public class ThanhToan
    {
        public int MaThanhToan { get; set; }
        
        [Display(Name = "Mã sinh viên")]
        public int MaSinhVien { get; set; }
        
        [Display(Name = "Mã khóa học")]
        public int MaKhoaHoc { get; set; }
        
        [Display(Name = "Số tiền")]
        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal SoTien { get; set; }
        
        [Display(Name = "Ngày thanh toán")]
        [Required(ErrorMessage = "Ngày thanh toán không được để trống")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayThanhToan { get; set; }
        
        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Chưa thanh toán";
        
        [Display(Name = "Phương thức thanh toán")]
        [StringLength(50)]
        public string? PhuongThucThanhToan { get; set; }
        
        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        // Navigation properties
        public virtual SinhVien? SinhVien { get; set; }
        public virtual KhoaHoc? KhoaHoc { get; set; }
    }
}