using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPassUpdateController : ControllerBase
    {
        private readonly IServiceUser _service;
        public UserPassUpdateController(IServiceUser service)
        {
            _service = service;
        }
        [HttpDelete("{id}")] //Deactivate user
        public async Task<IActionResult?> SoftDeleteUser(int id)
        {
            try
            {
                await _service.DeactivateUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
        [HttpPatch("{id}")] //Reactivate user
        public async Task<IActionResult> ReactivateUser(int id)
        {
            try
            {
                var user = await _service.ReactivateUserAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutPassword(int userId, ChangePasswordDto dto)
        {
            try
            {
                var user = await _service.ChangePasswordAsync(userId, dto);
                return Ok(user);
            }
            catch(KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
    }
}