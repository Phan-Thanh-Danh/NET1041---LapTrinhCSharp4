using Bai3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bai3.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            // Lấy dữ liệu sản phẩm đã được Explicit Loading thông tin Category
            var products = _productService.GetProductsWithCategories();
            return View(products);
        }
    }
}
