using System.Collections.Generic;

namespace Bai3.Models
{
    public class KhoaHoc
    {
        public int MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int SoTinChi { get; set; } = 3;
        public decimal HocPhi { get; set; }
        
        // Foreign Key
        public int? MaGiangVien { get; set; }
        
        // Navigation properties
        public virtual GiangVien? GiangVien { get; set; }
        public virtual ICollection<DangKyKhoaHoc>? DangKyKhoaHocs { get; set; }
        public virtual ICollection<ThanhToan>? ThanhToans { get; set; }
    }
}