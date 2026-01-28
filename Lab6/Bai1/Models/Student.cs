using System.ComponentModel.DataAnnotations;

namespace Bai1.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sinh viên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Batch Time không được để trống")]
        [DataType(DataType.Time)]
        public TimeSpan? BatchTime { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(
            @"^0\d{9}$",
            ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 0"
        )]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Website không được để trống")]
        [Url(ErrorMessage = "Website không đúng định dạng URL")]
        public string WebUrl { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 20 ký tự")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(255, ErrorMessage = "Địa chỉ tối đa 255 ký tự")]
        public string Address { get; set; } = null!;
    }
}
