// File: ViewModels/SachViewModel.cs
using System;

namespace bai2.ViewModels
{
    public class SachViewModel
    {
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public decimal GiaTien { get; set; }
        public string? MoTaChiTiet { get; set; }
        public bool CoAnhBia { get; set; }
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime NgayTao { get; set; }

        public string NgayTaoFormatted => NgayTao.ToString("dd/MM/yyyy HH:mm:ss");
        public string GiaTienFormatted => GiaTien.ToString("N0") + " đ";
    }
}
