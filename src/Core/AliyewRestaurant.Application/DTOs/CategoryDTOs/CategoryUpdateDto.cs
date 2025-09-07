namespace AliyewRestaurant.Application.DTOs.CategoryDTOs;

public record CategoryUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}