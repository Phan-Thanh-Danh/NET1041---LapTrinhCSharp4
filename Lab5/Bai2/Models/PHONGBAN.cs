using System.Collections.Generic;

namespace Bai2.Models
{
    public class PHONGBAN
    {
        public int Phongban_id { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        
        // Quan hệ một-nhiều: Một phòng có nhiều nhân viên
        public ICollection<NHANVIEN>? NhanViens { get; set; }
    }
}