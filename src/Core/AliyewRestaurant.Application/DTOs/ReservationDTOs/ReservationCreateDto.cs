namespace AliyewRestaurant.Application.DTOs.ReservationDTOs;

public class ReservationCreateDto
{
    public string UserId { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfPeople { get; set; }
    public string Notes { get; set; }
}