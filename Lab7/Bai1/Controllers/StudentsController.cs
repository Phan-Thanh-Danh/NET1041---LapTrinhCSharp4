using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bai1.Data;
using Bai1.Models;

namespace Bai1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students (with Search functionality)
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                // Tìm kiếm theo FirstName, LastName hoặc Email
                var sql = @"SELECT * FROM Students 
                           WHERE FirstName LIKE '%' + {0} + '%' 
                              OR LastName LIKE '%' + {0} + '%' 
                              OR Email LIKE '%' + {0} + '%'
                           ORDER BY StudentId";
                
                var students = await _context.Students
                    .FromSqlRaw(sql, searchString)
                    .ToListAsync();
                
                return View(students);
            }
            else
            {
                // Hiển thị tất cả students
                var students = await _context.Students
                    .FromSqlRaw("SELECT * FROM Students ORDER BY StudentId")
                    .ToListAsync();
                
                return View(students);
            }
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FromSqlRaw("SELECT * FROM Students WHERE StudentId = {0}", id)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sql = @"INSERT INTO Students (FirstName, LastName, DateOfBirth, Email) 
                               VALUES ({0}, {1}, {2}, {3})";
                    
                    await _context.Database.ExecuteSqlRawAsync(sql,
                        student.FirstName,
                        student.LastName,
                        student.DateOfBirth ?? (object)DBNull.Value,
                        student.Email ?? (object)DBNull.Value);

                    TempData["SuccessMessage"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating student: " + ex.Message);
                }
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FromSqlRaw("SELECT * FROM Students WHERE StudentId = {0}", id)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,DateOfBirth,Email")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sql = @"UPDATE Students 
                               SET FirstName = {0}, LastName = {1}, DateOfBirth = {2}, Email = {3}
                               WHERE StudentId = {4}";

                    await _context.Database.ExecuteSqlRawAsync(sql,
                        student.FirstName,
                        student.LastName,
                        student.DateOfBirth ?? (object)DBNull.Value,
                        student.Email ?? (object)DBNull.Value,
                        student.StudentId);

                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating student: " + ex.Message);
                }
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FromSqlRaw("SELECT * FROM Students WHERE StudentId = {0}", id)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sql = "DELETE FROM Students WHERE StudentId = {0}";
                await _context.Database.ExecuteSqlRawAsync(sql, id);

                TempData["SuccessMessage"] = "Student deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting student: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}