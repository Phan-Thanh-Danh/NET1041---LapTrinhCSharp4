namespace Bai3.Models
{
    public class DangKyKhoaHoc
    {
        public int MaSinhVien { get; set; }
        public int MaKhoaHoc { get; set; }
        public DateTime NgayDangKy { get; set; } = DateTime.Now;
        public string TrangThaiDangKy { get; set; } = "Đã đăng ký";
        
        // Navigation properties
        public virtual SinhVien? SinhVien { get; set; }
        public virtual KhoaHoc? KhoaHoc { get; set; }
    }
}