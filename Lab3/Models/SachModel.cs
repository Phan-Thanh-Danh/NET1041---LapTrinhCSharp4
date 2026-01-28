using System;

namespace bai2.Models
{
    public class SachModel
    {
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public decimal GiaTien { get; set; }
        public string MoTaChiTiet { get; set; }
        public byte[] AnhBia { get; set; }
        public int MaTheLoai { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime NgayTao { get; set; }

        // Navigation property (nếu cần)
        public string TenTheLoai { get; set; }
    }
}