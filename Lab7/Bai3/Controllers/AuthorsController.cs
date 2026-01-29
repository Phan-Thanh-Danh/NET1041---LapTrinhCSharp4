using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;

namespace Bai3.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppdbContext _context;

        public AuthorsController(AppdbContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            // Sử dụng LINQ to Entities để lấy danh sách tác giả kèm số lượng sách
            var authors = await _context.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.AuthorName)
                .ToListAsync();

            return View(authors);
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID tác giả không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để lấy chi tiết tác giả kèm danh sách sách
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tác giả";
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorName")] Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra tên tác giả đã tồn tại chưa
                    var existingAuthor = await _context.Authors
                        .FirstOrDefaultAsync(a => a.AuthorName.ToLower() == author.AuthorName.ToLower());

                    if (existingAuthor != null)
                    {
                        ModelState.AddModelError("AuthorName", "Tên tác giả đã tồn tại");
                        return View(author);
                    }

                    _context.Add(author);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm tác giả thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi thêm tác giả: {ex.Message}";
                }
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID tác giả không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để tìm tác giả
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tác giả";
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,AuthorName")] Author author)
        {
            if (id != author.AuthorId)
            {
                TempData["ErrorMessage"] = "ID không khớp";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra tên tác giả đã tồn tại chưa (trừ chính nó)
                    var existingAuthor = await _context.Authors
                        .FirstOrDefaultAsync(a => a.AuthorName.ToLower() == author.AuthorName.ToLower() 
                                                  && a.AuthorId != author.AuthorId);

                    if (existingAuthor != null)
                    {
                        ModelState.AddModelError("AuthorName", "Tên tác giả đã tồn tại");
                        return View(author);
                    }

                    _context.Update(author);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tác giả thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
                    {
                        TempData["ErrorMessage"] = "Tác giả không tồn tại";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi cập nhật tác giả: {ex.Message}";
                }
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID tác giả không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để tìm tác giả kèm danh sách sách
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tác giả";
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Sử dụng LINQ to Entities để tìm tác giả
                var author = await _context.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.AuthorId == id);

                if (author == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tác giả";
                    return RedirectToAction(nameof(Index));
                }

                // Kiểm tra tác giả có sách hay không
                if (author.Books.Any())
                {
                    TempData["ErrorMessage"] = $"Không thể xóa tác giả '{author.AuthorName}' vì còn {author.Books.Count} sách liên quan. Vui lòng xóa các sách trước.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa tác giả thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa tác giả: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}