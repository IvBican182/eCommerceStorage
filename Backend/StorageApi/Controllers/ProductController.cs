
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return product;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _productService.CreateProduct(product);
            return product;

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> UpdateProduct(Guid id, Product product)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(id, product);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}