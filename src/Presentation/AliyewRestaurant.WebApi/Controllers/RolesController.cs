using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.RoleDTOs;
using AliyewRestaurant.Application.Shared;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AliyewRestaurant.Application.Shared.Helpers;
using AliyewRestaurant.Application.Shared.Settings;
using Microsoft.AspNetCore.Authorization;
using static AliyewRestaurant.Application.Shared.Settings.Permissions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(Policy = Permissions.Role.GetAllPermissions)]
    [HttpGet("with-permissions")]
    [ProducesResponseType(typeof(BaseResponse<List<RoleWithPermissionsDto>>), 200)]
    public async Task<IActionResult> GetRolesWithPermissions()
    {
        var response = await _roleService.GetRolesWithPermissionsAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize(Policy = Permissions.Role.GetAllPermissions)]
    [HttpGet("permissions")]
    public IActionResult GetAllPermissions()
    {
        var permissions = PermissionHelper.GetAllPermissions();
        return Ok(permissions);
    }

    [Authorize(Policy = Permissions.Role.Create)]
    [HttpPost("create-role")]
    [ProducesResponseType(typeof(BaseResponse<string?>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string?>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string?>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
    {
        var result = await _roleService.CreateRole(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize(Policy = Role.Update)]
    [HttpPost("ad-to-role")]
    public async Task<IActionResult> AssignRole([FromQuery] string userId, [FromQuery] string roleName)
    {
        var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize(Policy = Permissions.Role.Delete)]
    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var response = await _roleService.DeleteRoleAsync(roleName);
        return StatusCode((int)response.StatusCode, response);
    }


}
