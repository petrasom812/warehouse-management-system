

using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Transfer
{
    public class CreateTransferDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int SourceWarehouseId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int DestinationWarehouseId { get; set; }
        public List<TransferItemDto> Item { get; set; } = new();
    }
}