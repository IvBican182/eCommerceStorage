

using StorageApi.Core.Models;

namespace StorageApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Guid id, Product product);
        Task<bool> DeleteProduct(Guid id);
    }
}