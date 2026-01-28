using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Bai2.Data;
using Bai2.Models;
using Bai2.ViewModels;
using System.Data;
using System.Text.Json;

namespace Bai2.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _context.Orders
                    .FromSqlRaw("EXEC sp_GetOrders")
                    .ToListAsync();

                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách đơn hàng: {ex.Message}";
                return View(new List<Order>());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã đơn hàng";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Lấy thông tin đơn hàng bằng sp_GetOrderById
                var orderIdParam = new SqlParameter("@OrderId", id);
                var orders = await _context.Orders
                    .FromSqlRaw("EXEC sp_GetOrderById @OrderId", orderIdParam)
                    .ToListAsync();

                var order = orders.FirstOrDefault();

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction(nameof(Index));
                }

                // Lấy chi tiết đơn hàng
                var orderDetailsParam = new SqlParameter("@OrderId", id);
                var orderDetails = await _context.OrderDetails
                    .FromSqlRaw("EXEC sp_GetOrderDetails @OrderId", orderDetailsParam)
                    .ToListAsync();

                order.OrderDetails = orderDetails;

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải chi tiết đơn hàng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var viewModel = new CreateOrderViewModel
            {
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetailInput>()
            };
            return View(viewModel);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin đã nhập";
                return View(viewModel);
            }

            if (viewModel.OrderDetails == null || !viewModel.OrderDetails.Any())
            {
                ModelState.AddModelError("", "Đơn hàng phải có ít nhất một sản phẩm");
                TempData["ErrorMessage"] = "Đơn hàng phải có ít nhất một sản phẩm";
                return View(viewModel);
            }

            try
            {
                // Tính tổng tiền
                decimal totalAmount = viewModel.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);

                // Chuyển OrderDetails thành JSON
                var orderDetailsJson = JsonSerializer.Serialize(viewModel.OrderDetails);

                // Tạo parameters
                var parameters = new[]
                {
                    new SqlParameter("@OrderDate", SqlDbType.DateTime) { Value = viewModel.OrderDate },
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = viewModel.CustomerName },
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = totalAmount, Precision = 18, Scale = 2 },
                    new SqlParameter("@OrderDetailsJson", SqlDbType.NVarChar, -1) { Value = orderDetailsJson },
                    new SqlParameter("@NewOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                // Gọi stored procedure
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_CreateOrder @OrderDate, @CustomerName, @TotalAmount, @OrderDetailsJson, @NewOrderId OUTPUT",
                    parameters);

                var newOrderId = (int)parameters[4].Value;

                TempData["SuccessMessage"] = $"Đơn hàng #{newOrderId} đã được tạo thành công!";
                return RedirectToAction(nameof(Details), new { id = newOrderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo đơn hàng: {ex.Message}";
                return View(viewModel);
            }
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã đơn hàng";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Lấy thông tin đơn hàng bằng sp_GetOrderById
                var orderIdParam = new SqlParameter("@OrderId", id);
                var orders = await _context.Orders
                    .FromSqlRaw("EXEC sp_GetOrderById @OrderId", orderIdParam)
                    .ToListAsync();

                var order = orders.FirstOrDefault();

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction(nameof(Index));
                }

                // Lấy chi tiết đơn hàng
                var orderDetailsParam = new SqlParameter("@OrderId", id);
                var orderDetails = await _context.OrderDetails
                    .FromSqlRaw("EXEC sp_GetOrderDetails @OrderId", orderDetailsParam)
                    .ToListAsync();

                order.OrderDetails = orderDetails;

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải thông tin đơn hàng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var orderIdParam = new SqlParameter("@OrderId", id);
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteOrder @OrderId", orderIdParam);

                TempData["SuccessMessage"] = "Đơn hàng đã được xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa đơn hàng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}