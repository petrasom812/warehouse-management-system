using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IServiceUserRole _service;
        public UserRoleController(IServiceUserRole service)
        {
            _service = service;
        }
        [HttpPost] //Create UserRole
        public async Task<ActionResult> PostUserRole(CreateUserRoleDto dto)
        {
            var userRole = await _service.CreateUserRolelAsync(dto);
            return Ok(userRole);
        }
        [HttpGet] //Get UserRole
        public async Task<ActionResult> GetUserRole()
        {
            var userRole = await _service.GetUserRoleAsync();
            return Ok(userRole);
        }
        [HttpGet("{id}")] //Get UserRole by Id
        public async Task<ActionResult> GetUserRoleById(int id)
        {
            var userRole = await _service.GetUserRoleByIdAsync(id);
            return userRole == null ? NotFound() : Ok(userRole);
        }
    }
}