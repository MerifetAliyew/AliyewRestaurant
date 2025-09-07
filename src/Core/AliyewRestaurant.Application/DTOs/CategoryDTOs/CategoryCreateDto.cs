namespace AliyewRestaurant.Application.DTOs.CategoryDTOs;

public record CategoryCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}