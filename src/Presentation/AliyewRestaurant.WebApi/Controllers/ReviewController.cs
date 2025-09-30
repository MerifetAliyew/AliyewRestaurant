using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.ReviewDTOs;
using AliyewRestaurant.Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AliyewRestaurant.Application.Shared.Settings;
using static AliyewRestaurant.Application.Shared.Settings.Permissions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly UserManager<AppUser> _userManager;

    public ReviewController(IReviewService reviewService, UserManager<AppUser> userManager)
    {
        _reviewService = reviewService;
        _userManager = userManager;
    }

    // Normal user üçün: review yaratmaq
    [HttpPost]
    [Authorize] // Yalnız login olan user
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto dto)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _reviewService.CreateReviewAsync(userId, dto);
        return StatusCode((int)result.StatusCode, result);
    }

    // Bütün review-ləri gətirmək (yalnız təsdiqlənmişləri göstəririk)
    [HttpGet]
    [Authorize] // Normal user + admin
    public async Task<IActionResult> GetAll([FromQuery] bool onlyConfirmed = true)
    {
        var result = await _reviewService.GetAllReviewsAsync(onlyConfirmed);
        return Ok(result);
    }

    // Admin üçün: review təsdiqləmək
    [HttpPut("{id}/confirm")]
    [Authorize(Policy = ReviewPermissions.Update)]
    public async Task<IActionResult> ConfirmReview(Guid id)
    {
        var result = await _reviewService.ConfirmReviewAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }
}
