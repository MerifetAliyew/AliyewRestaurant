namespace AliyewRestaurant.Application.DTOs.OrderDTOs;

public class OrderCreateDto
{
    public string UserId { get; set; }
    public List<OrderItemCreateDto> Items { get; set; }
}
