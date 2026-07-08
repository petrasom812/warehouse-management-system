using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;
using WMS.WarehouseTransferSystem.Api.Interfaces;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Warehouse
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IServiecWarehouse _service;
        public WarehouseController(IServiecWarehouse serviec)
        {
            _service = serviec;
        }
        [HttpPost] //Create Warehouse
        public async Task<ActionResult> PostWarehouse(CreateWarehouseDto dto)
        {
            var createWarehouse = await _service.CreateWarehouseAsync(dto);
            return Ok(createWarehouse);
        }
        [HttpGet] //Get Warehouse
        public async Task<ActionResult> GetWarehouse()
        {
            var getWarehouse = await _service.GetWarehouseDtoAsync();
            return Ok(getWarehouse);
        }
        [HttpGet("{id}")] //Get by Id
        public async Task<ActionResult> GetWarehouseById(int id)
        {
            var getWarehouseById = await _service.GetWarehouseByIdAsync(id);
            return getWarehouseById == null ? NotFound() : Ok(getWarehouseById);
        }
        [HttpPut("{id}")] //Update Warehose
        public async Task<IActionResult> PutWarehouse(int id, UpdateWarehouseDto dto)
        {
            var updateWarehouse = await _service.UpdateWarehouseAsync(id, dto);
            return updateWarehouse == null ? NotFound() : Ok(updateWarehouse);
        }
        [HttpDelete("{id}")] //Delete Warehouse
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            await _service.DeleteWarehouseAsync(id);
            return NoContent();
        }
    }
}