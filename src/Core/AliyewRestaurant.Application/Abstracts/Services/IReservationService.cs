using AliyewRestaurant.Application.DTOs.ReservationDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IReservationService
{
    Task<BaseResponse<ReservationGetDto>> CreateReservationAsync(ReservationCreateDto dto);
    Task<BaseResponse<List<ReservationGetDto>>> GetUserReservationsAsync(string userId);
    Task<BaseResponse<bool>> CancelReservationAsync(Guid reservationId, string userId);
    Task<BaseResponse<bool>> ConfirmReservationAsync(Guid reservationId);
    Task<BaseResponse<List<TableGetDto>>> GetAvailableTablesAsync(DateTime start, DateTime end, int numberOfPeople, TableType? type = null);
}
