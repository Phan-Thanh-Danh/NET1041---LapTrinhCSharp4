using Microsoft.AspNetCore.Mvc;
using Bai1.Services;
using Bai1.Models;
using System.Threading.Tasks;

namespace Bai1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var eagerResult = await _orderService.GetOrdersWithDishesEagerAsync();
            var sqlResult = await _orderService.GetOrdersWithDishesSqlAsync();

            var viewModel = new OrderComparisonViewModel
            {
                EagerLoadingResult = eagerResult,
                RawSqlResult = sqlResult
            };

            return View(viewModel);
        }
    }
}
