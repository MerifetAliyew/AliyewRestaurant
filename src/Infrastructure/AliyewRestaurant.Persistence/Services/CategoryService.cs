using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.CategoryDTOs;
using AliyewRestaurant.Application.Shared;
using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AliyewRestaurant.Domain.Entites;

namespace AliyewRestaurant.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse<CategoryGetDto>> CreateCategoryAsync(CategoryCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new BaseResponse<CategoryGetDto>("Ad boş ola bilməz", null, HttpStatusCode.BadRequest);

        var existing = await _categoryRepository.GetByFiltered(c => c.Name == dto.Name).FirstOrDefaultAsync();
        if (existing != null)
            return new BaseResponse<CategoryGetDto>("Bu adda kateqoriya artıq mövcuddur", null, HttpStatusCode.BadRequest);

        if (dto.ParentCategoryId.HasValue)
        {
            var group = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value);
            if (group == null)
                return new BaseResponse<CategoryGetDto>("Qrup kateqoriya tapılmadı", null, HttpStatusCode.NotFound);
        }

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();

        var resultDto = new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId
        };

        return new BaseResponse<CategoryGetDto>("Kateqoriya uğurla yaradıldı", resultDto, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<CategoryGetDto>> UpdateCategoryAsync(CategoryUpdateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);
        if (category == null)
            return new BaseResponse<CategoryGetDto>("Kateqoriya tapılmadı", null, HttpStatusCode.NotFound);

        if (dto.ParentCategoryId == dto.Id)
            return new BaseResponse<CategoryGetDto>("Kateqoriya özü öz qrupu ola bilməz", null, HttpStatusCode.BadRequest);

        if (dto.ParentCategoryId.HasValue)
        {
            var group = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value);
            if (group == null)
                return new BaseResponse<CategoryGetDto>("Qrup kateqoriya tapılmadı", null, HttpStatusCode.NotFound);
        }

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.ParentCategoryId = dto.ParentCategoryId;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangeAsync();

        var resultDto = new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId
        };

        return new BaseResponse<CategoryGetDto>("Kateqoriya uğurla yeniləndi", resultDto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByFiltered(
            c => c.Id == id,
            new Expression<Func<Category, object>>[] { c => c.SubCategories, c => c.MenuItems }
        ).FirstOrDefaultAsync();

        if (category == null)
            return new BaseResponse<string>("Kateqoriya tapılmadı", null, HttpStatusCode.NotFound);

        if (category.SubCategories.Any())
            return new BaseResponse<string>("Alt kateqoriyaları olan kateqoriya silinə bilməz", null, HttpStatusCode.BadRequest);

        if (category.MenuItems.Any())
            return new BaseResponse<string>("İçində yeməklər olan kateqoriya silinə bilməz", null, HttpStatusCode.BadRequest);

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>("Kateqoriya uğurla silindi", null, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByFiltered(
            c => c.Id == id,
            new Expression<Func<Category, object>>[] { c => c.SubCategories }
        ).FirstOrDefaultAsync();

        if (category == null)
            return new BaseResponse<CategoryGetDto>("Kateqoriya tapılmadı", null, HttpStatusCode.NotFound);

        return new BaseResponse<CategoryGetDto>("Kateqoriya tapıldı", MapCategoryToDto(category), HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAll().Include(c => c.SubCategories).ToListAsync();
        var dtos = categories.Select(MapCategoryToDto).ToList();
        return new BaseResponse<List<CategoryGetDto>>("Kateqoriya siyahısı", dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetTreeAsync()
    {
        var rootCategories = await _categoryRepository.GetAll()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories)
            .ToListAsync();

        var dtos = rootCategories.Select(MapCategoryToDto).ToList();
        return new BaseResponse<List<CategoryGetDto>>("Kateqoriya ağacı", dtos, HttpStatusCode.OK);
    }

    private CategoryGetDto MapCategoryToDto(Category category)
    {
        return new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            SubCategories = category.SubCategories?.Select(MapCategoryToDto).ToList() ?? new List<CategoryGetDto>()
        };
    }
}
