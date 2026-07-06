using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Inventory
{
    public class UpdateInventoryDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}