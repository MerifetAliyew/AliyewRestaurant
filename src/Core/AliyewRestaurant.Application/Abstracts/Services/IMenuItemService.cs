using AliyewRestaurant.Application.DTOs.MenuItemDTOs;
using AliyewRestaurant.Application.Shared;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IMenuItemService
{
    Task<BaseResponse<MenuItemGetDto>> CreateMenuItemAsync(MenuItemCreateDto dto);
    Task<BaseResponse<MenuItemGetDto>> UpdateMenuItemAsync(MenuItemUpdateDto dto);
    Task<BaseResponse<string>> DeleteMenuItemAsync(Guid id);
    Task<BaseResponse<MenuItemGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<MenuItemGetDto>>> GetAllAsync();
}