using AliyewRestaurant.Application.DTOs.RoleDTOs;
using AliyewRestaurant.Application.Shared;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IRoleService
{
    Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
    Task<BaseResponse<string>> DeleteRoleAsync(string roleName);
    Task<BaseResponse<List<string>>> GetAllRolesAsync();
    Task<BaseResponse<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync();
    Task<BaseResponse<string>> AssignRoleToUserAsync(string userId, string roleName);
}
