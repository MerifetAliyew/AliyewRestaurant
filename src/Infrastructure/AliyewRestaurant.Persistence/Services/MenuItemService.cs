using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.MenuItemDTOs;
using AliyewRestaurant.Application.Shared;
using System.Linq.Expressions;
using System.Net;
using AliyewRestaurant.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace AliyewRestaurant.Persistence.Services;

public class MenuItemService : IMenuItemService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemService(ICategoryRepository categoryRepository, IMenuItemRepository menuItemRepository)
    {
        _categoryRepository = categoryRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<BaseResponse<MenuItemGetDto>> CreateMenuItemAsync(MenuItemCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new BaseResponse<MenuItemGetDto>("Ad boş ola bilməz", HttpStatusCode.BadRequest);

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return new BaseResponse<MenuItemGetDto>("Kateqoriya tapılmadı", HttpStatusCode.NotFound);

        var menuItem = new MenuItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            IsAvailable = dto.IsAvailable
        };

        await _menuItemRepository.AddAsync(menuItem);
        await _menuItemRepository.SaveChangeAsync();

        var resultDto = MapMenuItemToDto(menuItem, category.Name);

        return new BaseResponse<MenuItemGetDto>("Menyu elementi uğurla yaradıldı", resultDto, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<MenuItemGetDto>> UpdateMenuItemAsync(MenuItemUpdateDto dto)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(dto.Id);
        if (menuItem == null)
            return new BaseResponse<MenuItemGetDto>("Menyu elementi tapılmadı", HttpStatusCode.NotFound);

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return new BaseResponse<MenuItemGetDto>("Kateqoriya tapılmadı", HttpStatusCode.NotFound);

        menuItem.Name = dto.Name;
        menuItem.Description = dto.Description;
        menuItem.Price = dto.Price;
        menuItem.CategoryId = dto.CategoryId;
        menuItem.IsAvailable = dto.IsAvailable;

        _menuItemRepository.Update(menuItem);
        await _menuItemRepository.SaveChangeAsync();

        var resultDto = MapMenuItemToDto(menuItem, category.Name);

        return new BaseResponse<MenuItemGetDto>("Menyu elementi uğurla yeniləndi", resultDto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteMenuItemAsync(Guid id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
            return new BaseResponse<string>("Menyu elementi tapılmadı", HttpStatusCode.NotFound);

        _menuItemRepository.Delete(menuItem);
        await _menuItemRepository.SaveChangeAsync();

        // ✅ Burada sadəcə mesaj və status kod qaytarıram
        return new BaseResponse<string>("Menyu elementi uğurla silindi", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<MenuItemGetDto>> GetByIdAsync(Guid id)
    {
        var menuItem = await _menuItemRepository.GetByFiltered(
            m => m.Id == id,
            new Expression<Func<MenuItem, object>>[] { m => m.Category }
        ).FirstOrDefaultAsync();

        if (menuItem == null)
            return new BaseResponse<MenuItemGetDto>("Menyu elementi tapılmadı", null, HttpStatusCode.NotFound);

        var resultDto = MapMenuItemToDto(menuItem, menuItem.Category?.Name ?? "");
        return new BaseResponse<MenuItemGetDto>("Menyu elementi tapıldı", resultDto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<MenuItemGetDto>>> GetAllAsync()
    {
        var menuItems = await _menuItemRepository.GetAll()
            .Include(m => m.Category)
            .ToListAsync();

        if (!menuItems.Any())
            return new BaseResponse<List<MenuItemGetDto>>("Menyu elementləri tapılmadı", new List<MenuItemGetDto>(), HttpStatusCode.OK);

        var dtos = menuItems.Select(m => MapMenuItemToDto(m, m.Category?.Name ?? "")).ToList();

        return new BaseResponse<List<MenuItemGetDto>>("Menyu elementləri tapıldı", dtos, HttpStatusCode.OK);
    }

    private MenuItemGetDto MapMenuItemToDto(MenuItem menuItem, string categoryName)
    {
        return new MenuItemGetDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            IsAvailable = menuItem.IsAvailable,
            CategoryId = menuItem.CategoryId,
            CategoryName = categoryName
        };
    }
}
