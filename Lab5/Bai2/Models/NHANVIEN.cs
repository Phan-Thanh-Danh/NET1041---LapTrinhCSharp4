using System.Collections.Generic;

namespace Bai2.Models
{
    public class NHANVIEN
    {
        public int Nhanvien_id { get; set; }
        public string Nhanvien_name { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; } = string.Empty;
        public bool GioiTinh { get; set; }
        public decimal Luong { get; set; } // Đổi từ float sang decimal

        // Khóa ngoại đến PHONGBAN
        public int? PHG { get; set; }
        public PHONGBAN? PhongBan { get; set; }

        // Khóa ngoại đến NGUOIQULY - SỬA THÀNH int?
        public int? Ma_NQL { get; set; } // Đổi tên từ MA_NQL -> Ma_NQL và kiểu int?
        public NGUOIQULY? NguoiQuanLy { get; set; }

        // Quan hệ một-nhiều đến THANNHAN
        public ICollection<THANNHAN>? ThanNhans { get; set; }
    }
}