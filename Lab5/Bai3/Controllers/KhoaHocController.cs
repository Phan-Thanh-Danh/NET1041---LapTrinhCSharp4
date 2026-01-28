using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bai3.Controllers
{
    public class KhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhoaHocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KhoaHoc
        public async Task<IActionResult> Index()
        {
            var khoaHocs = await _context.KhoaHocs
                .Include(k => k.GiangVien)
                .ToListAsync();
            return View(khoaHocs);
        }

        // GET: KhoaHoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.GiangVien)
                .Include(k => k.DangKyKhoaHocs)
                    .ThenInclude(d => d.SinhVien)
                .Include(k => k.ThanhToans)
                .FirstOrDefaultAsync(m => m.MaKhoaHoc == id);

            if (khoaHoc == null)
            {
                return NotFound();
            }

            return View(khoaHoc);
        }

        // GET: KhoaHoc/Create
        public IActionResult Create()
        {
            ViewData["MaGiangVien"] = new SelectList(_context.GiangViens, "MaGiangVien", "TenGiangVien");
            return View();
        }

        // POST: KhoaHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhoaHoc,TenKhoaHoc,NgayBatDau,NgayKetThuc,SoTinChi,HocPhi,MaGiangVien")] KhoaHoc khoaHoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khoaHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGiangVien"] = new SelectList(_context.GiangViens, "MaGiangVien", "TenGiangVien", khoaHoc.MaGiangVien);
            return View(khoaHoc);
        }

        // GET: KhoaHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc == null)
            {
                return NotFound();
            }
            ViewData["MaGiangVien"] = new SelectList(_context.GiangViens, "MaGiangVien", "TenGiangVien", khoaHoc.MaGiangVien);
            return View(khoaHoc);
        }

        // POST: KhoaHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhoaHoc,TenKhoaHoc,NgayBatDau,NgayKetThuc,SoTinChi,HocPhi,MaGiangVien")] KhoaHoc khoaHoc)
        {
            if (id != khoaHoc.MaKhoaHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoaHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoaHocExists(khoaHoc.MaKhoaHoc))
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
            ViewData["MaGiangVien"] = new SelectList(_context.GiangViens, "MaGiangVien", "TenGiangVien", khoaHoc.MaGiangVien);
            return View(khoaHoc);
        }

        // GET: KhoaHoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.GiangVien)
                .FirstOrDefaultAsync(m => m.MaKhoaHoc == id);
            if (khoaHoc == null)
            {
                return NotFound();
            }

            return View(khoaHoc);
        }

        // POST: KhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc != null)
            {
                _context.KhoaHocs.Remove(khoaHoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoaHocExists(int id)
        {
            return _context.KhoaHocs.Any(e => e.MaKhoaHoc == id);
        }
    }
}