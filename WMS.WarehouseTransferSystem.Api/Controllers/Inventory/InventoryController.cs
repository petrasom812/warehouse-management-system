using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;
using WMS.WarehouseTransferSystem.Api.Interfaces.Inventory;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Inventory
{
    [Authorize(Roles = "InventoryControl, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly  IServiceInventory _service;
        public InventoryController(IServiceInventory service)
        {
            _service = service;
        }
        //Create Inventory
        [HttpPost]
        public async Task<ActionResult> PostInventory(CreateInventoryDto dto)
        {
            try
            {
                var createInventory = await _service.CreateInventoryAsync(dto);
                return Ok(createInventory);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var getInventoryById = await _service.GetInventoryByIdAsync(id);
                return Ok(getInventoryById);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")] //Update Inventory
        public async Task<IActionResult> PutInventory(int id, UpdateInventoryDto dto)
        {
            try
            {
                var updateInventory = await _service.UpdateInventoryAsync(id, dto);
                return Ok(updateInventory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")] //delete inventory
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                await _service.DeleteInventoryAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}