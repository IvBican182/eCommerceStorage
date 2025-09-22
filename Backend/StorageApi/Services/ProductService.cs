

using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            return products;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} wasn't found");

            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, Product product)
        {
            var productToUpdate = await GetProductById(id);

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.Price = product.Price;

            await _context.SaveChangesAsync();

            return productToUpdate;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var productToDelete = await GetProductById(id);

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}