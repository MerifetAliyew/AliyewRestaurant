namespace AliyewRestaurant.Application.DTOs.OrderDTOs;

public class OrderItemDto
{
    public Guid MenuItemId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}