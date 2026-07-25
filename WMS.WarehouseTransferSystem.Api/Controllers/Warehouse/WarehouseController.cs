using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;
using WMS.WarehouseTransferSystem.Api.Interfaces.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Warehouse
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IServiceWarehouse _serivce;
        public WarehouseController(IServiceWarehouse service)
        {
            _serivce = service;
        }
        [HttpPost] //Create Warehouse
        public async Task<ActionResult> PostWarehouse(CreateWarehouseDto dto)
        {
            try
            {
                var warehouse = await _serivce.CreateWarehouseAsync(dto);
                return Ok(warehouse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet] //Get Warehouse
        public async Task<ActionResult> GetWarehouse()
        {
            var warehouse = await _serivce.GetWarehousesAsync();
            return Ok(warehouse);
        }
        [HttpGet("{id}")] //Get by Id
        public async Task<ActionResult> GetWarehouseById(int id)
        {
            try
            {
                var warehouse = await _serivce.GetWarehouseByIdAsync(id);
                return Ok(warehouse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")] //Update Warehose
        public async Task<IActionResult> PutWarehouse(int id, UpdateWarehouseDto dto)
        {
            try
            {
                var warehouse = await _serivce.UpdateWarehouseAsync(id, dto);
                return Ok(warehouse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")] //Delete Warehouse
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            try
            {
                var warehouse = await _serivce.DeleteWarehouseAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}