using System.Collections.Generic;

namespace Bai3.Models
{
    public class SinhVien
    {
        public int MaSinhVien { get; set; }
        public string HoVaTen { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public DateTime? NgaySinh { get; set; }
        
        // Navigation properties
        public virtual ICollection<DangKyKhoaHoc>? DangKyKhoaHocs { get; set; }
        public virtual ICollection<ThanhToan>? ThanhToans { get; set; }
    }
}