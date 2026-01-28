using System.Collections.Generic;

namespace Bai2.Models
{
    public class NGUOIQULY
    {
        public int NguoiQuanLy_id { get; set; }
        public string TenQuanLy { get; set; } = string.Empty;
        
        // Quan hệ một-nhiều: Một người quản lý có thể quản lý nhiều nhân viên
        public ICollection<NHANVIEN>? NhanViens { get; set; }
    }
}