using System.Net;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.UserDTOs;
using AliyewRestaurant.Application.Shared;
using Microsoft.AspNetCore.Mvc;

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
}
