using AliyewRestaurant.Application.Abstracts.Repositories;
using System.Linq.Expressions;
using System.Net;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.OrderDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AliyewRestaurant.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public OrderService(
        IOrderRepository orderRepository,
        IRepository<MenuItem> menuItemRepository,
        UserManager<AppUser> userManager,
        IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<BaseResponse<OrderGetDto>> CreateOrderAsync(OrderCreateDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            return new BaseResponse<OrderGetDto>("Sifariş ən azı 1 elementdən ibarət olmalıdır.", HttpStatusCode.BadRequest);

        // yalnız Quantity > 0 olan itemləri götürürük
        var validItems = dto.Items.Where(i => i.Quantity > 0).ToList();

        if (!validItems.Any())
            return new BaseResponse<OrderGetDto>("Sifariş ən azı 1 ədəd olmalıdır.", HttpStatusCode.BadRequest);

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
            return new BaseResponse<OrderGetDto>("İstifadəçi tapılmadı", HttpStatusCode.NotFound);

        var order = new Order
        {
            UserId = dto.UserId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            OrderItems = new List<OrderItem>(),
            TotalAmount = 0m
        };

        foreach (var item in validItems)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
            if (menuItem == null || !menuItem.IsAvailable)
                return new BaseResponse<OrderGetDto>($"Menyu elementi {item.MenuItemId} mövcud deyil", HttpStatusCode.BadRequest);

            var itemTotal = item.Quantity * menuItem.Price;

            order.OrderItems.Add(new OrderItem
            {
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity,
                TotalPrice = itemTotal
            });

            order.TotalAmount += itemTotal;
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangeAsync();

        if (!string.IsNullOrEmpty(user.Email))
        {
            await _emailService.SendEmailAsync(
                new List<string> { user.Email },
                "Sifariş yaradıldı",
                $"Sizin {order.Id} nömrəli sifarişiniz uğurla yaradıldı. Ümumi məbləğ: {order.TotalAmount} AZN."
            );
        }

        var orderDto = MapOrderToDto(order, user.FullName);
        return new BaseResponse<OrderGetDto>("Sifariş uğurla yaradıldı", orderDto, HttpStatusCode.Created);
    }


    public async Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository
            .GetByFiltered(o => o.Id == orderId, new Expression<Func<Order, object>>[] { o => o.OrderItems })
            .FirstOrDefaultAsync();

        if (order == null)
            return new BaseResponse<OrderGetDto>("Sifariş tapılmadı", HttpStatusCode.NotFound);

        var user = await _userManager.FindByIdAsync(order.UserId);
        return new BaseResponse<OrderGetDto>("Uğurla əldə edildi", MapOrderToDto(order, user?.FullName ?? "Naməlum"), HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository
            .GetAllFiltered(null, new Expression<Func<Order, object>>[] { o => o.OrderItems })
            .ToListAsync();

        var ordersDto = new List<OrderGetDto>();
        foreach (var order in orders)
        {
            var user = await _userManager.FindByIdAsync(order.UserId);
            ordersDto.Add(MapOrderToDto(order, user?.FullName ?? "Naməlum"));
        }

        return new BaseResponse<List<OrderGetDto>>("Uğurla əldə edildi", ordersDto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<string>("Sifariş tapılmadı", HttpStatusCode.NotFound);

        order.Status = status;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        var user = await _userManager.FindByIdAsync(order.UserId);
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            var mesaj = $"Sifariş: {status.ToAzeriMessage()}";
            await _emailService.SendEmailAsync(
                new List<string> { user.Email },
                "Sifariş statusu yeniləndi",
                mesaj
            );
        }

        return new BaseResponse<string>("Sifariş statusu uğurla yeniləndi", HttpStatusCode.OK);
    }

    private OrderGetDto MapOrderToDto(Order order, string userName)
    {
        return new OrderGetDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserName = userName,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                MenuItemId = oi.MenuItemId,
                Name = oi.MenuItem?.Name ?? "Naməlum",
                Quantity = oi.Quantity,
                TotalPrice = oi.TotalPrice
            }).ToList()
        };
    }
}
