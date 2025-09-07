namespace AliyewRestaurant.Application.DTOs.RoleDTOs;

public record class RoleCreateDto
{
    public string Name { get; set; } = null!;
    public List<string> PermissionsList { get; set; }
}