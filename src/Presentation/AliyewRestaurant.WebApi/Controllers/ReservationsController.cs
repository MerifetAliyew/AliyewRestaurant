using System.Net;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.ReservationDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Application.Shared.Settings;
using AliyewRestaurant.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(BaseResponse<ReservationGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<ReservationGetDto>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReservationGetDto>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<ReservationGetDto>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto dto)
    {
        var response = await _reservationService.CreateReservationAsync(dto);
        return StatusCode((int)response.StatusCode, response);
    }


    // 2️⃣ İstifadəçi rezervasiyaları
    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReservations(string userId)
    {
        var response = await _reservationService.GetUserReservationsAsync(userId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 3️⃣ Rezervasiyanı ləğv etmək

    [Authorize(Policy = Permissions.Reservation.View)]
    [HttpPut("{reservationId}/cancel/{userId}")]
    public async Task<IActionResult> CancelReservation(Guid reservationId, string userId)
    {
        var response = await _reservationService.CancelReservationAsync(reservationId, userId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 4️⃣ Rezervasiyanı təsdiqləmək
    [Authorize(Policy = Permissions.Reservation.View)]
    [HttpPut("{reservationId}/confirm")]
    public async Task<IActionResult> ConfirmReservation(Guid reservationId)
    {
        var response = await _reservationService.ConfirmReservationAsync(reservationId);
        return StatusCode((int)response.StatusCode, response);
    }

    // 5️⃣ Boş masaları göstərmək
    [HttpGet("available-tables")]
    public async Task<IActionResult> GetAvailableTables(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        [FromQuery] int numberOfPeople,
        [FromQuery] TableType? type = null)
    {
        var response = await _reservationService.GetAvailableTablesAsync(start, end, numberOfPeople, type);
        return StatusCode((int)response.StatusCode, response);
    }
}
