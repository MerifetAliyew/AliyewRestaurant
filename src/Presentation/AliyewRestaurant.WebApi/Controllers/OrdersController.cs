using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.OrderDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AliyewRestaurant.Application.Shared.Settings;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Policy = Permissions.Order.View)]
    [HttpGet("get")]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrderById([FromQuery] Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        // Token-dan user ID götür və DTO-dakı userId-ni override et
        var tokenUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(tokenUserId))
            return StatusCode(404, new BaseResponse<OrderGetDto>("İstifadəçi tapılmadı", HttpStatusCode.NotFound));

        dto.UserId = tokenUserId; // front-end-dən gələn userId override olunur

        var result = await _orderService.CreateOrderAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }



    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize(Policy = Permissions.Order.View)]
    [HttpPatch("update-status")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateOrderStatus([FromQuery] Guid id, [FromQuery] OrderStatus status)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, status);
        return StatusCode((int)result.StatusCode, result);
    }
}
