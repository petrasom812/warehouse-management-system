using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Warehouse
{
    public class UpdateWarehouseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string WarehouseCode { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string WarehouseName { get; set; } = string.Empty;
    }
}