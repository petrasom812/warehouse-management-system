using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Product;
using WMS.WarehouseTransferSystem.Api.Interfaces.Product;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Product
{
    [Authorize(Roles = "Admin")]
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
            try
            {
                var createProduct = await _service.CreateProductAsync(dto);
                return Ok(createProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var getProductById = await _service.GetProductByIdAsync(id);
                return Ok(getProductById);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")] //Update Product
        public async Task<IActionResult> PutProduct(int id, UpdateProductDto dto)
        {
            try
            {
                var updateProduct = await _service.UpdateProductAsync(id, dto);
                return Ok(updateProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")] //Delete Product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}