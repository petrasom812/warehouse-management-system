using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            await _service.DeactivateUserAsync(id);
            return NoContent();
        }
        [HttpPatch("{id}")] //Reactivate user
        public async Task<IActionResult> ReactivateUser(int id)
        {
            var user = await _service.ReactivateUserAsync(id);
            return !user ? NotFound() : Ok(user);
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutPassword(int userId, ChangePasswordDto dto)
        {
            var user = await _service.ChangePasswordAsync(userId, dto);
            return user == null ? NotFound() : Ok(user);
        }
    }   
}