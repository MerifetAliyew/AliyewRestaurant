using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Domain.Entites;

public class Table : BaseEntity
{
    public int TableNumber { get; set; }
    public int Seats { get; set; }
    public bool IsAvailable { get; set; } = true;
    public TableType Type { get; set; } = TableType.Regular; // ✅ əlavə etdik
    public ICollection<Reservation> Reservations { get; set; }
}