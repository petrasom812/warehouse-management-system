using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Role;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Auth
{
    public interface IServiceRole
    {
        Task<GetRoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task<List<GetRoleDto>> GetRoleAsync();
        Task<GetRoleDto?> GetRoleByIdAsync(int id);
        Task<UpdateRoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto);
        Task<bool> DeleteRoleAsync(int id);
    }
}