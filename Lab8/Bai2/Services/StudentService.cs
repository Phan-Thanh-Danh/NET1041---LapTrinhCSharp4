using Bai2.Data;
using Bai2.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai2.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            // Với Lazy Loading, chúng ta không cần .Include(s => s.Courses)
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }
    }
}
