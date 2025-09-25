

using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;

namespace StorageApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.GetRepository<Product>();
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _productRepository.GetAll().ToListAsync();

            return products;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} wasn't found");

            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, Product product)
        {
            var productToUpdate = await GetProductById(id);

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.Price = product.Price;

            _productRepository.Update(productToUpdate);

            await _unitOfWork.SaveChangesAsync();

            return productToUpdate;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var productToDelete = await GetProductById(id);

            _productRepository.Remove(productToDelete);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}