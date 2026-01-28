using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;

namespace Bai3.Controllers
{
    public class GiangVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GiangVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GiangVien
        public async Task<IActionResult> Index()
        {
            return View(await _context.GiangViens.ToListAsync());
        }

        // GET: GiangVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giangVien = await _context.GiangViens
                .Include(g => g.KhoaHocs)
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);

            if (giangVien == null)
            {
                return NotFound();
            }

            return View(giangVien);
        }

        // GET: GiangVien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiangVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGiangVien,TenGiangVien,Email,SoDienThoai,HocVi,ChuyenNganh")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giangVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giangVien);
        }

        // GET: GiangVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giangVien = await _context.GiangViens.FindAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGiangVien,TenGiangVien,Email,SoDienThoai,HocVi,ChuyenNganh")] GiangVien giangVien)
        {
            if (id != giangVien.MaGiangVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giangVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiangVienExists(giangVien.MaGiangVien))
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
            return View(giangVien);
        }

        // GET: GiangVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giangVien = await _context.GiangViens
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (giangVien == null)
            {
                return NotFound();
            }

            return View(giangVien);
        }

        // POST: GiangVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giangVien = await _context.GiangViens.FindAsync(id);
            if (giangVien != null)
            {
                _context.GiangViens.Remove(giangVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiangVienExists(int id)
        {
            return _context.GiangViens.Any(e => e.MaGiangVien == id);
        }
    }
}