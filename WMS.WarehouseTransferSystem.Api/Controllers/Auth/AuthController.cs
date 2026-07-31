using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.LoginDto;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;

namespace WMS.WarehouseTransferSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceAuth _service;
        public AuthController(IServiceAuth service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost]//Login
        public async Task<ActionResult> PostLogIn(LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(token);
        }
        [Authorize]
        [HttpGet("adminuser")]
        public IActionResult Admin()
        {
            return Ok("Adminuser");
        }

    }
}