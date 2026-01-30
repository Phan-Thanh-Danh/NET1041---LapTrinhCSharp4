using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bai3.Data;
using Bai3.Models;

namespace Bai3.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppdbContext _context;

        public BooksController(AppdbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? authorId)
        {
            // Sử dụng LINQ to Entities để lấy danh sách sách
            var booksQuery = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            // SỬ DỤNG .Where() ĐỂ LỌC THEO TÁC GIẢ
            if (authorId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.AuthorId == authorId);
                ViewBag.CurrentAuthorId = authorId;
            }

            var books = await booksQuery
                .OrderBy(b => b.BookTitle)
                .ToListAsync();

            // Lấy danh sách tác giả cho dropdown lọc
            ViewBag.Authors = new SelectList(
                await _context.Authors.OrderBy(a => a.AuthorName).ToListAsync(),
                "AuthorId",
                "AuthorName",
                authorId
            );

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID sách không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để lấy chi tiết sách
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            // Sử dụng LINQ to Entities để lấy danh sách tác giả
            ViewBag.Authors = new SelectList(
                _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                "AuthorId",
                "AuthorName"
            );
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookTitle,PublicationYear,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra tác giả có tồn tại không
                    var authorExists = await _context.Authors
                        .AnyAsync(a => a.AuthorId == book.AuthorId);

                    if (!authorExists)
                    {
                        ModelState.AddModelError("AuthorId", "Tác giả không tồn tại");
                        ViewBag.Authors = new SelectList(
                            _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                            "AuthorId",
                            "AuthorName",
                            book.AuthorId
                        );
                        return View(book);
                    }

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm sách thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi thêm sách: {ex.Message}";
                }
            }

            ViewBag.Authors = new SelectList(
                _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                "AuthorId",
                "AuthorName",
                book.AuthorId
            );
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID sách không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để tìm sách
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new SelectList(
                _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                "AuthorId",
                "AuthorName",
                book.AuthorId
            );
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookTitle,PublicationYear,AuthorId")] Book book)
        {
            if (id != book.BookId)
            {
                TempData["ErrorMessage"] = "ID không khớp";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra tác giả có tồn tại không
                    var authorExists = await _context.Authors
                        .AnyAsync(a => a.AuthorId == book.AuthorId);

                    if (!authorExists)
                    {
                        ModelState.AddModelError("AuthorId", "Tác giả không tồn tại");
                        ViewBag.Authors = new SelectList(
                            _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                            "AuthorId",
                            "AuthorName",
                            book.AuthorId
                        );
                        return View(book);
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật sách thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        TempData["ErrorMessage"] = "Sách không tồn tại";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi cập nhật sách: {ex.Message}";
                }
            }

            ViewBag.Authors = new SelectList(
                _context.Authors.OrderBy(a => a.AuthorName).ToList(),
                "AuthorId",
                "AuthorName",
                book.AuthorId
            );
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID sách không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng LINQ to Entities để tìm sách
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Sử dụng LINQ to Entities để tìm và xóa sách
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa sách thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sách";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa sách: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}