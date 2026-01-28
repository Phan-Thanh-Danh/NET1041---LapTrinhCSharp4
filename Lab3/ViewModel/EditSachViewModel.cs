// File: ViewModels/EditSachViewModel.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bai2.ViewModels
{
    public class EditSachViewModel
    {
        public int MaSach { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề sách")]
        public string TieuDe { get; set; } = null!;

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [MaxLength(100, ErrorMessage = "Tác giả không được vượt quá 100 ký tự")]
        [Display(Name = "Tác giả")]
        public string TacGia { get; set; } = null!;

        [Required(ErrorMessage = "Giá tiền không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá tiền")]
        public decimal GiaTien { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả chi tiết")]
        public string? MoTaChiTiet { get; set; }

        [Display(Name = "Ảnh bìa mới")]
        public IFormFile? AnhBiaFile { get; set; }

        public bool CoAnhBia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        [Display(Name = "Thể loại")]
        public int MaTheLoai { get; set; }

        [Required(ErrorMessage = "Số lượng tồn không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Số lượng tồn")]
        public int SoLuongTon { get; set; }

        // Danh sách thể loại cho dropdown
        public List<SelectListItem> TheLoaiList { get; set; } = new List<SelectListItem>();
    }
}