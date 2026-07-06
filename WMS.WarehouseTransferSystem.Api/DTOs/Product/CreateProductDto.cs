using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Sku { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ProductName { get; set; } = string.Empty;
    }
}