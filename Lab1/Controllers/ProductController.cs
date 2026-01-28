using Microsoft.AspNetCore.Mvc;
using Lab1.Models;

namespace Lab1.Controllers;

public class ProductController : Controller
{
    private static List<Product> _products = new List<Product>
    {
        new Product { ProductId = 1, ProductName = "Laptop", Price = 1200.50m, Stock = 10 },
        new Product { ProductId = 2, ProductName = "Smartphone", Price = 799.00m, Stock = 25 },
        new Product { ProductId = 3, ProductName = "Headphones", Price = 150.00m, Stock = 50 }
    };

    [HttpGet]
    public IActionResult Index()
    {
        return GetProducts();
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        return View("Index", _products);
    }

    [HttpGet]
    // Attribute Routing: Lab requirement /Product/Details/{id}
    [Route("Product/Details/{id}")]
    public IActionResult Details(int id)
    {
        var product = _products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            TempData["Error"] = "Không tìm thấy sản phẩm!";
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    // Attribute Routing: Lab requirement /Product/Search/{name}
    [Route("Product/Search/{name}")]
    public IActionResult Search(string name)
    {
        // Simple search just filtering by name containment
        var results = _products.Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        return View("Index", results); // Reuse Index view to show results
    }

    [HttpGet] // Helper to show form, not explicitly asked but needed for UI
    public IActionResult Create()
    {
        return View("Editor", new Product());
    }
    
    [HttpGet] // Helper to show form
    public IActionResult Edit(int id)
    {
        var p = _products.FirstOrDefault(x => x.ProductId == id);
        if (p == null) return RedirectToAction("Index");
        return View("Editor", p);
    }

    // Lab Requirement: ProductOperation: [AcceptVerbs("POST", "PUT")]
    // If ProductId has value -> update, else -> create
    [AcceptVerbs("POST", "PUT")]
    public IActionResult ProductOperation(Product product)
    {
        if (!ModelState.IsValid)
        {
             return View("Editor", product);
        }

        var existing = _products.FirstOrDefault(p => p.ProductId == product.ProductId);
        
        if (existing == null && product.ProductId == 0)
        {
            // Create
            product.ProductId = _products.Count > 0 ? _products.Max(p => p.ProductId) + 1 : 1;
            _products.Add(product);
            TempData["Success"] = "Tạo sản phẩm thành công!";
        }
        else if (existing != null)
        {
            // Update
            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            TempData["Success"] = "Cập nhật sản phẩm thành công!";
        }
        else
        {
             // Case: ProductId provided but not found (create new with that ID? or error?)
             // Usually treated as Create if ID is not critical to be sequential, but here simple logic:
             TempData["Error"] = "Không tìm thấy sản phẩm để cập nhật.";
        }

        return RedirectToAction("Index");
    }

    [HttpDelete] // Lab Requirement
    public IActionResult RemoveProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.ProductId == id);
        if (product != null)
        {
            _products.Remove(product);
            return Ok(); // API-style delete
        }
        return NotFound();
    }

    // POST wrapper for Form submit to delete (since forms don't do DELETE easily)
    [HttpPost] 
    public IActionResult Delete(int id)
    {
         var product = _products.FirstOrDefault(p => p.ProductId == id);
        if (product != null)
        {
            _products.Remove(product);
            TempData["Success"] = "Đã xóa sản phẩm!";
        }
        else {
             TempData["Error"] = "Không tìm thấy sản phẩm!";
        }
        return RedirectToAction("Index");
    }
}