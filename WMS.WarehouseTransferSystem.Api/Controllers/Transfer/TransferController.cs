using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.WarehouseTransferSystem.Api.DTOs.Transfer;
using WMS.WarehouseTransferSystem.Api.Interfaces.Transfer;

namespace WMS.WarehouseTransferSystem.Api.Controllers.Transfer
{
    [Authorize(Roles = "Admin, Operator")]
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IServiceTransfer _service;
        public TransferController(IServiceTransfer service)
        {
            _service = service;           
        }
        [HttpPost] //Create Transfer
        public async Task<ActionResult> PostTransfer(CreateTransferDto dto)
        {
            var createTransfer = await _service.CreateTransferAsync(dto);
            return Ok(createTransfer);
        }
        [HttpGet] // Get Transfer
        public async Task<ActionResult> GetTransfer()
        {
            var getTrasfer = await _service.GetTransferAsync();
            return Ok(getTrasfer);
        }
        [HttpGet("{id}")] //Get Transfer by Id
        public async Task<ActionResult> GetTransferById(int id)
        {
            var getTransferById = await _service.GetTransferByIdAsync(id);
            return getTransferById == null ? NotFound() : Ok(getTransferById);
        }
    }
}