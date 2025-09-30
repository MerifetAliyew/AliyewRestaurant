using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Application.DTOs.ReservationDTOs;

public class ReservationGetDto
{
    public Guid Id { get; set; }
    public int TableNumber { get; set; }
    public string? UserName { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfPeople { get; set; }
    public string Notes { get; set; }
    public ReservationStatus Status { get; set; }
}