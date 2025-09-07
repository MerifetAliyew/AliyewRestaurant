using AliyewRestaurant.Application.DTOs.CategoryDTOs;
using AliyewRestaurant.Application.Shared;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<BaseResponse<CategoryGetDto>> CreateCategoryAsync(CategoryCreateDto dto);
    Task<BaseResponse<CategoryGetDto>> UpdateCategoryAsync(CategoryUpdateDto dto);
    Task<BaseResponse<string>> DeleteCategoryAsync(Guid id);
    Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync();
    Task<BaseResponse<List<CategoryGetDto>>> GetTreeAsync();
}
