using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Application.DTOs.ReservationDTOs;

public class TableGetDto
{
    public Guid Id { get; set; }
    public int TableNumber { get; set; }
    public int Seats { get; set; }
    public TableType Type { get; set; }
    public bool IsAvailable { get; set; }
}