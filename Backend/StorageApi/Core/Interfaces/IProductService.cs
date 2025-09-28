

using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Guid id, Product product);
        Task<DeleteProductResponseDto> DeleteProduct(Guid id);
    }
}