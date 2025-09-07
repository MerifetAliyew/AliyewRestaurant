namespace AliyewRestaurant.Application.DTOs.MenuItemDTOs;

public record MenuItemCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsAvailable { get; set; } = true;
}