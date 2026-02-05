using System.Collections.Generic;
using System.Linq;
using Bai3.Data;
using Bai3.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai3.Services
{
    public interface IProductService
    {
        List<Product> GetProductsWithCategories();
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProductsWithCategories()
        {
            var products = _context.Products.ToList();

            foreach (var product in products)
            {
                _context.Entry(product).Reference(p => p.Category).Load();
            }

            return products;
        }
    }
}
