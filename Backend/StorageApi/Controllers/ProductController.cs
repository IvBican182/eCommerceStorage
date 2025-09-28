
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            var productDto = _mapper.Map<ProductDto>(product);

            if (productDto == null)
            { 
                return NotFound( new { error = "product not found" });
            }

            return Ok(productDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct(Product product)
        {

            var cretedProduct = await _productService.CreateProduct(product);
            var createdProductDto = _mapper.Map<ProductDto>(cretedProduct);

            if (createdProductDto == null)
            {
                return BadRequest(new { error = "invalid inputs" });
            }

            return Ok(createdProductDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, Product product)
        {
            var updatedProduct = await _productService.UpdateProduct(id, product);
            var updatedProductDto = _mapper.Map<ProductDto>(updatedProduct);

            if (updatedProductDto == null)
            {
                return NotFound( new { error = "product wasn't updated" });
            }

            return Ok(updatedProductDto);  
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleteProductResponse = await _productService.DeleteProduct(id);

            if (!deleteProductResponse.Success)
            {
                return NotFound(new { error = deleteProductResponse.Error });
            }

            return NoContent();     
        }
    }
}