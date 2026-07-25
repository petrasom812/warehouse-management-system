using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IServiceUser _service;
        public UserController(IServiceUser service)
        {
            _service = service;
        }
        [HttpPost] //Create User
        public async Task<ActionResult> PostUser(CreateUserDto dto)
        {
            var user = await _service.CreateUserAsync(dto);
            return Ok(user);
        }
        [HttpGet] //Get Users
        public async Task<ActionResult> GetUser()
        {
            var user = await _service.GetUserAsync();
            return Ok(user);
        }
        [HttpGet("{id}")] //Get user by id
        public async Task<ActionResult> GetUserById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
        [HttpPut("{id}")] // update user
        public async Task<IActionResult> PutUser(int id, UpdateUserDto dto)
        {
            var user = await _service.UpdateUserAsync(id, dto);
            return user == null ? NotFound() : Ok(user);
        }
        [HttpDelete("{id}")] //Hard delete user
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.DeleteUserAsync(id);
            return NoContent();
        }
    }
}