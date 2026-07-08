using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;
using WMS.WarehouseTransferSystem.Api.Interfaces;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Inventory
{
    [Authorize(Roles = "InventoryControl, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IServiceInventory _service;
        public InventoryController(IServiceInventory service)
        {
            _service = service;
        }
        //Create Inventory
        [HttpPost]
        public async Task<ActionResult> PostInventory(CreateInventoryDto dto)
        {
            var createInventory = await _service.CreateInventoryAsync(dto);
            return Ok(createInventory);
        }
        [HttpGet] //Get Inventory
        public async Task<ActionResult> GetInventory()
        {
            var getInventory = await _service.GetInventoryAsync();
            return Ok(getInventory);
        }
        [HttpGet("{id}")] //Get Inventory by Id
        public async Task<ActionResult> GetInventoryById(int id)
        {
            var getInventoryById = await _service.GetProductByIdAsync(id);
            return getInventoryById == null ? NotFound() : Ok(getInventoryById);
        }
        [HttpPut("{id}")] //Update Inventory
        public async Task<IActionResult> PutInventory(int id, UpdateInventoryDto dto)
        {
            var updateInventory = await _service.UpdateInventoryAsync(id, dto);
            return updateInventory == null ? NotFound() : Ok(updateInventory);
        }
        [HttpDelete("{id}")] //delete inventory
        public async Task<IActionResult> DeleteInventory(int id)
        {
            await _service.DeleteInventoryAsync(id);
            return NoContent();
        }
    }
}