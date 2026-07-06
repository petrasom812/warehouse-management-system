using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Product;
using WMS.WarehouseTransferSystem.Api.Interfaces.Product;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProduct _service;
        public ProductController(IServiceProduct service)
        {
            _service = service;
        }
        [HttpPost] //CreateProduct
        public async Task<ActionResult> PostProduct(CreateProductDto dto)
        {
            var createProduct = await _service.CreateProductAsync(dto);
            return Ok(createProduct);
        }
        [HttpGet] //Get Product
        public async Task<ActionResult> GetProduct()
        {
            var getProduct = await _service.GetProductAsync();
            return Ok(getProduct);
        }
        [HttpGet("{id}")] //Get Product by Id
        public async Task<ActionResult> GetProductById(int id)
        {
            var getProductById = await _service.GetProductByIdAsync(id);
            return getProductById == null ? NotFound() : Ok(getProductById);
        }
        [HttpPut("{id}")] //Update Product
        public async Task<IActionResult> PutProduct(int id, UpdateProductDto dto)
        {
            var updateProduct = await _service.UpdateProductAsync(id, dto);
            return updateProduct == null ? NotFound() : Ok(updateProduct);
        }
        [HttpDelete("{id}")] //Delete Product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
    }
}