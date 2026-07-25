using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Warehouse
{
    public class CreateWarehouseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        [RegularExpression(@"^WH-", ErrorMessage = "The field must start with 'WH-'.")]
        public string WarehouseCode { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 20)]
        public string WarehouseName { get; set; } = string.Empty;
    }
}