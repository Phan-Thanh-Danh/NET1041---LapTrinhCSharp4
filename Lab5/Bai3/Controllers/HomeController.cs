using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;

namespace Bai3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Thống kê
            ViewBag.SinhVienCount = await _context.SinhViens.CountAsync();
            ViewBag.GiangVienCount = await _context.GiangViens.CountAsync();
            ViewBag.KhoaHocCount = await _context.KhoaHocs.CountAsync();
            ViewBag.ThanhToanCount = await _context.ThanhToans.CountAsync();

            // Hoạt động gần đây
            ViewBag.RecentDangKy = await _context.DangKyKhoaHocs
                .Include(d => d.SinhVien)
                .Include(d => d.KhoaHoc)
                .OrderByDescending(d => d.NgayDangKy)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentThanhToan = await _context.ThanhToans
                .Include(t => t.SinhVien)
                .Include(t => t.KhoaHoc)
                .OrderByDescending(t => t.NgayThanhToan)
                .Take(5)
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}