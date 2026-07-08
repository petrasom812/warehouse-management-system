using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Auth
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}