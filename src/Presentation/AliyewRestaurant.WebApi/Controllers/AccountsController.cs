using System.Net;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.UserDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Application.Shared.Settings;
using AliyewRestaurant.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AliyewRestaurant.Application.Shared.Settings.Permissions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;
    public AccountsController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var result = await _userService.Register(dto);
        return StatusCode((int)result.StatusCode, result);
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var result = await _userService.Login(dto);
        return StatusCode((int)result.StatusCode, result);
    }


    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        var result = await _userService.ForgotPasswordAsync(email);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _userService.RefreshTokenAsync(request);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _userService.ConfirmEmail(userId, token);
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize(Policy = Permissions.Role.GetAllPermissions)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(BaseResponse<List<UserListDto>>), 200)]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _userService.GetAllUsersAsync();
        return StatusCode((int)response.StatusCode, response);
    }


}
