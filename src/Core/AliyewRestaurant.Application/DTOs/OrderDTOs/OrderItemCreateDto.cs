namespace AliyewRestaurant.Application.DTOs.OrderDTOs;

public class OrderItemCreateDto
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
}