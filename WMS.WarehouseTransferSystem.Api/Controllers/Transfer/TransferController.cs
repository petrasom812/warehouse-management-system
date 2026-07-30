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
            try
            {
                var createTransfer = await _service.CreateTransferAsync(dto);
                return Ok(createTransfer);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
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
            try
            {
                var getTransferById = await _service.GetTransferByIdAsync(id);
                return getTransferById == null ? NotFound() : Ok(getTransferById);
            }
            catch(KeyNotFoundException ke)
            {
                return NotFound(ke.Message);
            }
        }
    }
}