using System.Collections.Generic;

namespace Bai3.Models
{
    public class GiangVien
    {
        public int MaGiangVien { get; set; }
        public string TenGiangVien { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? HocVi { get; set; }
        public string? ChuyenNganh { get; set; }
        
        // Navigation property
        public virtual ICollection<KhoaHoc>? KhoaHocs { get; set; }
    }
}