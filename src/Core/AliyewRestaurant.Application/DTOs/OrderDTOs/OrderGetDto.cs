using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Application.DTOs.OrderDTOs;

public class OrderGetDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; }
}