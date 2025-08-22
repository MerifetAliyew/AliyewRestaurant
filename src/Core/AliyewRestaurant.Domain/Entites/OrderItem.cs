namespace AliyewRestaurant.Domain.Entites;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; } // Quantity * MenuItem.Price
}
