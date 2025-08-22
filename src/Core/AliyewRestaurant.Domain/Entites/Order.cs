using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Domain.Entites;

public class Order : BaseEntity
{
    public string UserId { get; set; }  
    public AppUser User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}