using System.Net;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.ReservationDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Enums;
using AliyewRestaurant.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Application.Abstracts.Repositories;
using System.Linq.Expressions;


namespace AliyewRestaurant.Persistence.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRepository<Table> _tableRepository;
    private readonly IEmailService _emailService;

    public ReservationService(
        IReservationRepository reservationRepository,
        IRepository<Table> tableRepository,
        IEmailService emailService)
    {
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
        _emailService = emailService;
    }

    // 1️⃣ Rezervasiya yaratmaq
    public async Task<BaseResponse<ReservationGetDto>> CreateReservationAsync(ReservationCreateDto dto)
    {
        var reservation = new Reservation
        {
            UserId = dto.UserId,
            ReservedAt = dto.ReservedAt,
            EndTime = dto.EndTime,
            NumberOfPeople = dto.NumberOfPeople,
            Notes = dto.Notes,
            Status = ReservationStatus.Pending
        };

        await _reservationRepository.AddAsync(reservation);
        await _reservationRepository.SaveChangeAsync();

        await _emailService.SendEmailAsync(
            new List<string> { "user@example.com" },
            "Rezervasiya təsdiqi",
            "Rezervasiyanız yaradıldı və Pending statusundadır."
        );

        var resultDto = new ReservationGetDto
        {
            Id = reservation.Id,
            UserName = reservation.User?.UserName,
            ReservedAt = reservation.ReservedAt,
            EndTime = reservation.EndTime,
            NumberOfPeople = reservation.NumberOfPeople,
            Notes = reservation.Notes,
            Status = reservation.Status
        };

        return new BaseResponse<ReservationGetDto>("Rezervasiya uğurla yaradıldı.", resultDto, HttpStatusCode.OK);
    }


    // 2️⃣ İstifadəçinin rezervasiyalarını göstərmək
    public async Task<BaseResponse<List<ReservationGetDto>>> GetUserReservationsAsync(string userId)
    {
        // Pending → Expired yoxlaması
        var pendingReservations = _reservationRepository
            .GetByFiltered(r => r.Status == ReservationStatus.Pending && r.ReservedAt.AddHours(24) < DateTime.UtcNow)
            .ToList();

        foreach (var r in pendingReservations)
        {
            r.Status = ReservationStatus.Expired;
            _reservationRepository.Update(r);
        }
        await _reservationRepository.SaveChangeAsync();

        // İstifadəçi rezervasiyaları
        var reservations = _reservationRepository
            .GetByFiltered(r => r.UserId == userId)
            .OrderByDescending(r => r.ReservedAt)
            .ToList();

        var result = reservations.Select(r => new ReservationGetDto
        {
            Id = r.Id,
            UserName = r.User?.UserName,
            ReservedAt = r.ReservedAt,
            EndTime = r.EndTime,
            NumberOfPeople = r.NumberOfPeople,
            Notes = r.Notes,
            Status = r.Status
        }).ToList();

        return new BaseResponse<List<ReservationGetDto>>("İstifadəçinin rezervasiyaları göstərildi.", result, HttpStatusCode.OK);
    }


    // 3️⃣ Rezervasiyanı ləğv etmək
    public async Task<BaseResponse<bool>> CancelReservationAsync(Guid reservationId, string userId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return new BaseResponse<bool>("Rezervasiya tapılmadı.", HttpStatusCode.NotFound);

        if (reservation.UserId != userId)
            return new BaseResponse<bool>("Bu rezervasiyanı ləğv etmək icazəniz yoxdur.", HttpStatusCode.Forbidden);

        reservation.Status = ReservationStatus.Cancelled;
        _reservationRepository.Update(reservation);
        await _reservationRepository.SaveChangeAsync();

        return new BaseResponse<bool>("Rezervasiya ləğv edildi.", true, HttpStatusCode.OK);
    }

    // 4️⃣ Rezervasiyanı təsdiqləmək
    public async Task<BaseResponse<bool>> ConfirmReservationAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return new BaseResponse<bool>("Rezervasiya tapılmadı.", HttpStatusCode.NotFound);

        reservation.Status = ReservationStatus.Confirmed;
        _reservationRepository.Update(reservation);
        await _reservationRepository.SaveChangeAsync();

        await _emailService.SendEmailAsync(
            new List<string> { "user@example.com" },
            "Rezervasiya təsdiqləndi",
            "Rezervasiyanız təsdiqləndi."
        );

        return new BaseResponse<bool>("Rezervasiya təsdiqləndi.", true, HttpStatusCode.OK);
    }

    // 5️⃣ Boş masaları göstərmək
    public async Task<BaseResponse<List<TableGetDto>>> GetAvailableTablesAsync(DateTime start, DateTime end, int numberOfPeople, TableType? type = null)
    {
        var tablesQuery = _tableRepository.GetAll(true);

        if (type.HasValue)
            tablesQuery = tablesQuery.Where(t => t.Type == type.Value);

        var tables = tablesQuery.Include(t => t.Reservations).ToList();

        var availableTables = tables.Where(t =>
            t.Seats >= numberOfPeople &&
            !t.Reservations.Any(r =>
                r.Status != ReservationStatus.Cancelled &&
                r.ReservedAt < end &&
                r.EndTime > start
            )
        ).Select(t => new TableGetDto
        {
            Id = t.Id,
            TableNumber = t.TableNumber,
            Seats = t.Seats,
            Type = t.Type,
            IsAvailable = true
        }).ToList();

        return new BaseResponse<List<TableGetDto>>("Boş masalar göstərildi.", availableTables, HttpStatusCode.OK);
    }
}
