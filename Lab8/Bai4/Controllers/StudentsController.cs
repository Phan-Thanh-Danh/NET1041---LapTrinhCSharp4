using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bai4.Data;
using Bai4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bai4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Eager Loading: Sử dụng Include() để lấy học sinh cùng môn học
            var eagerLoadingStudents = await _context
                .Students.Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            // 2. Lazy Loading: Sẽ được kích hoạt trong View khi truy cập student.Enrollments
            // Chúng ta chỉ cần lấy danh sách học sinh (không Include)
            var lazyLoadingStudents = await _context.Students.ToListAsync();

            // 3. SQL Thuần: Lấy danh sách học sinh, môn học và số lượng học sinh đăng ký mỗi môn
            var rawSqlStats = await _context
                .Courses.FromSqlRaw(
                    @"
                    SELECT c.CourseId, c.Title, COUNT(e.EnrollmentId) as StudentCount
                    FROM Courses c
                    LEFT JOIN Enrollments e ON c.CourseId = e.CourseId
                    GROUP BY c.CourseId, c.Title"
                )
                .Select(c => new CourseStatViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    StudentCount = _context.Enrollments.Count(e => e.CourseId == c.CourseId),
                })
                .ToListAsync();

            var viewModel = new StudentIndexViewModel
            {
                EagerStudents = eagerLoadingStudents,
                LazyStudents = lazyLoadingStudents,
                CourseStats = rawSqlStats,
            };

            return View(viewModel);
        }
    }

    public class StudentIndexViewModel
    {
        public List<Student> EagerStudents { get; set; } = new();
        public List<Student> LazyStudents { get; set; } = new();
        public List<CourseStatViewModel> CourseStats { get; set; } = new();
    }

    public class CourseStatViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public int StudentCount { get; set; }
    }
}
