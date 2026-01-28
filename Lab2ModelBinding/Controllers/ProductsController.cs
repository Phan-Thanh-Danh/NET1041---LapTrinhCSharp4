using Lab2ModelBinding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2ModelBinding.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products/Search
        public async Task<IActionResult> Search([FromQuery] ProductSearchModel search)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(p => p.Name.Contains(search.Name));
            }

            if (search.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= search.MinPrice.Value);
            }

            if (search.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= search.MaxPrice.Value);
            }

            var products = await query.ToListAsync();
            ViewBag.SearchModel = search;
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Id == product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Search));
            }
            return View(product);
        }
    }
}
