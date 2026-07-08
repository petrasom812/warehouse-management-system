
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
            var role = await _service.CreateRoleAsync(dto);
            return Ok(role);
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
            var role = await _service.GetRoleByIdAsync(id);
            return role == null ? NotFound() : Ok(role);
        }
        [HttpPut("{id}")] //update role
        public async Task<IActionResult> PutRole(int id, UpdateRoleDto dto)
        {
            var role = await _service.UpdateRoleAsync(id, dto);
            return role == null ? NotFound() : Ok(role);
        }
        [HttpDelete("{id}")] // delete role
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _service.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}