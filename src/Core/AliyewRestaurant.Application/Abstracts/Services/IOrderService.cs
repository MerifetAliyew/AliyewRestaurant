using AliyewRestaurant.Application.DTOs.OrderDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Enums;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IOrderService
{
    Task<BaseResponse<OrderGetDto>> CreateOrderAsync(OrderCreateDto dto);
    Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(Guid orderId);
    Task<BaseResponse<List<OrderGetDto>>> GetAllOrdersAsync();
    Task<BaseResponse<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
}