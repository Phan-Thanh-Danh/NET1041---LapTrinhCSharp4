namespace Bai2.Models
{
    public class THANNHAN
    {
        public int ThanNhan_id { get; set; }
        public string TenThanNhan { get; set; } = string.Empty;
        public bool GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string QuanHe { get; set; } = string.Empty;

        // Khóa ngoại đến NHANVIEN
        public int Nhanvien_id { get; set; }
        public NHANVIEN? NhanVien { get; set; }
    }
}