using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.OrderDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("get")]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrderById([FromQuery] Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        var result = await _orderService.CreateOrderAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return StatusCode((int)result.StatusCode, result);
    }

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
