using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.RoleDTOs;
using AliyewRestaurant.Application.Shared.Helpers;
using AliyewRestaurant.Application.Shared;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using AliyewRestaurant.Domain.Entites;

namespace AliyewRestaurant.Persistence.Services;

public class RoleService : IRoleService
{
    private readonly UserManager<AppUser> _userManager;
    private RoleManager<IdentityRole> _roleManager { get; }

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto)
    {
        var existingRole = await _roleManager.FindByNameAsync(dto.Name);
        if (existingRole != null)
        {
            return new BaseResponse<string?>("This role already exists", HttpStatusCode.BadRequest);
        }

        var allPermissions = PermissionHelper.GetPermissionList();
        var invalidPermissions = dto.PermissionsList.Except(allPermissions).ToList();

        if (invalidPermissions.Any())
        {
            return new BaseResponse<string?>(
                $"Invalid permissions: {string.Join(", ", invalidPermissions)}",
                HttpStatusCode.BadRequest
            );
        }

        var newRole = new IdentityRole(dto.Name);
        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string?>(errors, HttpStatusCode.BadRequest);
        }

        foreach (var permission in dto.PermissionsList)
        {
            await _roleManager.AddClaimAsync(newRole, new Claim("Permission", permission));
        }

        return new BaseResponse<string?>("Role created successfully", true, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return new BaseResponse<string>($"Role '{roleName}' tapılmadı.", HttpStatusCode.NotFound);
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string>($"Role silinə bilmədi: {errors}", HttpStatusCode.BadRequest);
        }

        return new BaseResponse<string>("Role uğurla silindi.", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<string>>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();

        return new BaseResponse<List<string>>("Rollar siyahısı", roles, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync()
    {
        var roles = _roleManager.Roles.ToList();
        var result = new List<RoleWithPermissionsDto>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            // CreatedAt claim olaraq saxlanıbsa onu oxuya bilərik, yoxdursa default DateTime.MinValue olacaq
            var createdAtClaim = claims.FirstOrDefault(c => c.Type == "CreatedAt")?.Value;
            var createdAt = createdAtClaim != null ? DateTime.Parse(createdAtClaim) : DateTime.MinValue;

            result.Add(new RoleWithPermissionsDto
            {
                Id = role.Id,
                Name = role.Name,
                CreatedAt = createdAt,
                Permissions = permissions
            });
        }

        return new BaseResponse<List<RoleWithPermissionsDto>>("Rollar və permissionlar siyahısı", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId); // burda artıq _userManager mövcuddur
        if (user == null)
            return new BaseResponse<string>("İstifadəçi tapılmadı.", null, HttpStatusCode.NotFound);

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
            return new BaseResponse<string>("Role mövcud deyil.", null, HttpStatusCode.BadRequest);

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string>($"Role təyin edilərkən xəta baş verdi: {errors}", null, HttpStatusCode.InternalServerError);
        }

        return new BaseResponse<string>($"Role '{roleName}' uğurla təyin edildi.", null, HttpStatusCode.OK);
    }
}