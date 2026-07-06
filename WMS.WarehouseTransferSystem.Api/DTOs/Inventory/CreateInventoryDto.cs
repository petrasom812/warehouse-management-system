using System.ComponentModel.DataAnnotations;


namespace WMS.WarehouseTransferSystem.Api.DTOs.Inventory
{
    public class CreateInventoryDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int WarehouseId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}