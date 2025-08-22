using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Domain.Entites;

public class Reservation : BaseEntity
{
    public Guid TableId { get; set; }
    public Table Table { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public DateTime ReservedAt { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfPeople { get; set; }
    public string Notes { get; set; }
    public ReservationStatus Status { get; set; }
}
