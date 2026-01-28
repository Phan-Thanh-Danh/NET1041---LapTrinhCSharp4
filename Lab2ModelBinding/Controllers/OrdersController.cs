using Lab2ModelBinding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2ModelBinding.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders/Filter
        public async Task<IActionResult> Filter([FromQuery] OrderFilterModel filter)
        {
            var query = _context.Orders.Include(o => o.OrderDetails).AsQueryable();

            if (filter.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= filter.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(o => o.Status == filter.Status);
            }

            var orders = await query.ToListAsync();
            ViewBag.FilterModel = filter;
            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Order order)
        {
            // Manual validation for demonstration if needed, but Model Binding handles nested lists if named correctly
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã tạo đơn hàng thành công!";
                return RedirectToAction(nameof(Filter));
            }
            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
