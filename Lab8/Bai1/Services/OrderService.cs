using Microsoft.EntityFrameworkCore;
using Bai1.Models;
using Bai1.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bai1.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersWithDishesEagerAsync();
        Task<List<OrderDetailViewModel>> GetOrdersWithDishesSqlAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersWithDishesEagerAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dishes)
                .ToListAsync();
        }

        public async Task<List<OrderDetailViewModel>> GetOrdersWithDishesSqlAsync()
        {
            var sql = @"
                SELECT o.OrderId, c.Name as CustomerName, o.OrderDate, d.Name as DishName
                FROM Orders o
                JOIN Customers c ON o.CustomerId = c.CustomerId
                JOIN Dishes d ON o.OrderId = d.OrderId";
            return await _context.Database
                .SqlQueryRaw<OrderDetailViewModel>(sql)
                .ToListAsync();
        }
    }
}
