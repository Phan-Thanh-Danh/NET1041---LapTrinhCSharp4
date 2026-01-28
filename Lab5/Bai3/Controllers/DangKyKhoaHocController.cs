using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bai3.Controllers
{
    public class DangKyKhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangKyKhoaHocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DangKyKhoaHoc
        public async Task<IActionResult> Index()
        {
            var dangKyKhoaHocs = await _context.DangKyKhoaHocs
                .Include(d => d.KhoaHoc)
                .Include(d => d.SinhVien)
                .ToListAsync();
            return View(dangKyKhoaHocs);
        }

        // GET: DangKyKhoaHoc/Details/5
        public async Task<IActionResult> Details(int? maSinhVien, int? maKhoaHoc)
        {
            if (maSinhVien == null || maKhoaHoc == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs
                .Include(d => d.KhoaHoc)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(m => m.MaSinhVien == maSinhVien && m.MaKhoaHoc == maKhoaHoc);

            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }

            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Create
        public IActionResult Create()
        {
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc");
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen");
            return View();
        }

        // POST: DangKyKhoaHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSinhVien,MaKhoaHoc,NgayDangKy,TrangThaiDangKy")] DangKyKhoaHoc dangKyKhoaHoc)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra đã đăng ký chưa
                var existing = await _context.DangKyKhoaHocs
                    .AnyAsync(d => d.MaSinhVien == dangKyKhoaHoc.MaSinhVien && d.MaKhoaHoc == dangKyKhoaHoc.MaKhoaHoc);

                if (existing)
                {
                    ModelState.AddModelError(string.Empty, "Sinh viên đã đăng ký khóa học này!");
                    ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", dangKyKhoaHoc.MaKhoaHoc);
                    ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", dangKyKhoaHoc.MaSinhVien);
                    return View(dangKyKhoaHoc);
                }

                _context.Add(dangKyKhoaHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", dangKyKhoaHoc.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", dangKyKhoaHoc.MaSinhVien);
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Edit/5
        public async Task<IActionResult> Edit(int? maSinhVien, int? maKhoaHoc)
        {
            if (maSinhVien == null || maKhoaHoc == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs.FindAsync(maSinhVien, maKhoaHoc);
            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", dangKyKhoaHoc.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", dangKyKhoaHoc.MaSinhVien);
            return View(dangKyKhoaHoc);
        }

        // POST: DangKyKhoaHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maSinhVien, int maKhoaHoc, [Bind("MaSinhVien,MaKhoaHoc,NgayDangKy,TrangThaiDangKy")] DangKyKhoaHoc dangKyKhoaHoc)
        {
            if (maSinhVien != dangKyKhoaHoc.MaSinhVien || maKhoaHoc != dangKyKhoaHoc.MaKhoaHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dangKyKhoaHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DangKyKhoaHocExists(dangKyKhoaHoc.MaSinhVien, dangKyKhoaHoc.MaKhoaHoc))
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
            ViewData["MaKhoaHoc"] = new SelectList(_context.KhoaHocs, "MaKhoaHoc", "TenKhoaHoc", dangKyKhoaHoc.MaKhoaHoc);
            ViewData["MaSinhVien"] = new SelectList(_context.SinhViens, "MaSinhVien", "HoVaTen", dangKyKhoaHoc.MaSinhVien);
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Delete/5
        public async Task<IActionResult> Delete(int? maSinhVien, int? maKhoaHoc)
        {
            if (maSinhVien == null || maKhoaHoc == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs
                .Include(d => d.KhoaHoc)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(m => m.MaSinhVien == maSinhVien && m.MaKhoaHoc == maKhoaHoc);

            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }

            return View(dangKyKhoaHoc);
        }

        // POST: DangKyKhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maSinhVien, int maKhoaHoc)
        {
            var dangKyKhoaHoc = await _context.DangKyKhoaHocs.FindAsync(maSinhVien, maKhoaHoc);
            if (dangKyKhoaHoc != null)
            {
                _context.DangKyKhoaHocs.Remove(dangKyKhoaHoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DangKyKhoaHocExists(int maSinhVien, int maKhoaHoc)
        {
            return _context.DangKyKhoaHocs.Any(e => e.MaSinhVien == maSinhVien && e.MaKhoaHoc == maKhoaHoc);
        }
    }
}