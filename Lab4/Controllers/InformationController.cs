using Microsoft.AspNetCore.Mvc;
using Lab4.Models; // Đảm bảo namespace này khớp với thư mục Models của bạn
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Controllers
{
    // Bỏ cái [Route("[controller]")] đi để dùng routing mặc định cho dễ (localhost:xxx/Information/Index)
    public class InformationController : Controller
    {
        // Khởi tạo context để làm việc với DB
        private readonly CompanyContext _context;

        public InformationController(CompanyContext context)
        {
            _context = context;
        }

        // Bài 3: Action Index để hiển thị danh sách
        public IActionResult Index()
        {
            var data = _context.Informations.ToList();
            return View(data);
        }

        // Bài 1: Thêm 1 record mẫu vào bảng Information
        public ActionResult CreateInformation()
        {
            var info = new Information()
            {
                Name = "Phan Viet The",
                License = "Thepv",
                Revenue = 1000,
                Establshied = new System.DateTime(2024, 9, 2)
            };
            _context.Entry(info).State = EntityState.Added;
            
            _context.SaveChanges();

            // Sau khi thêm xong, chuyển hướng về trang danh sách để xem kết quả
            return RedirectToAction("Index");
        }

        // (Tùy chọn) Bài 3: Trang tạo mới thủ công
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Information info)
        {
            if (ModelState.IsValid)
            {
                _context.Informations.Add(info);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(info);
        }
    }
}