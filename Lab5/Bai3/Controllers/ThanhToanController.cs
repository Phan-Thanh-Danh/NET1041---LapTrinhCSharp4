using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bai3.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThanhToan
        public async Task<IActionResult> Index()
        {
            var thanhToans = await _context.ThanhToans
                .Include(t => t.KhoaHoc)
                .Include(t => t.SinhVien)
                .ToListAsync();
            return View(thanhToans);
        }

        // GET: ThanhToan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.KhoaHoc)
                .Include(t => t.SinhVien)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // GET: ThanhToan/Create
        public IActionResult Create()
        {
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc");
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen");
            return View();
        }

        // POST: ThanhToan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThanhToan,MaSinhVien,MaKhoaHoc,SoTien,NgayThanhToan,TrangThai,PhuongThucThanhToan,GhiChu")] ThanhToan thanhToan)
        {
            // Kiểm tra ngày thanh toán không lớn hơn ngày hiện tại
            if (thanhToan.NgayThanhToan > DateTime.Now)
            {
                ModelState.AddModelError("NgayThanhToan", "Ngày thanh toán không được lớn hơn ngày hiện tại");
            }

            if (ModelState.IsValid)
            {
                _context.Add(thanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", thanhToan.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", thanhToan.MaSinhVien);
            return View(thanhToan);
        }

        // GET: ThanhToan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.KhoaHoc)
                .Include(t => t.SinhVien)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);
                
            if (thanhToan == null)
            {
                return NotFound();
            }
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", thanhToan.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", thanhToan.MaSinhVien);
            return View(thanhToan);
        }

        // POST: ThanhToan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThanhToan,MaSinhVien,MaKhoaHoc,SoTien,NgayThanhToan,TrangThai,PhuongThucThanhToan,GhiChu")] ThanhToan thanhToan)
        {
            if (id != thanhToan.MaThanhToan)
            {
                return NotFound();
            }

            // Kiểm tra ngày thanh toán không lớn hơn ngày hiện tại
            if (thanhToan.NgayThanhToan > DateTime.Now)
            {
                ModelState.AddModelError("NgayThanhToan", "Ngày thanh toán không được lớn hơn ngày hiện tại");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.MaThanhToan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", thanhToan.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", thanhToan.MaSinhVien);
            return View(thanhToan);
        }

        // GET: ThanhToan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.KhoaHoc)
                .Include(t => t.SinhVien)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // POST: ThanhToan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(int id)
        {
            return _context.ThanhToans.Any(e => e.MaThanhToan == id);
        }
    }
}