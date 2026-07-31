
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Role;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IServiceRole _service;
        public RoleController(IServiceRole service)
        {
            _service = service;
        }
        [HttpPost]//Create Role
        public async Task<ActionResult> PostRole(CreateRoleDto dto)
        {
            try
            {
                var role = await _service.CreateRoleAsync(dto);
                return Ok(role);
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]//Get role
        public async Task<ActionResult> GetRole()
        {
            var role = await _service.GetRoleAsync();
            return Ok(role);
        }
        [HttpGet("{id}")]//Get Role by Id
        public async Task<ActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _service.GetRoleByIdAsync(id);
                return Ok(role);
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
        [HttpPut("{id}")] //update role
        public async Task<IActionResult> PutRole(int id, UpdateRoleDto dto)
        {
            try
            {
                var role = await _service.UpdateRoleAsync(id, dto);
                return Ok(role);
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")] // delete role
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _service.DeleteRoleAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
    }
}