namespace AliyewRestaurant.Application.DTOs.RoleDTOs;

public class RoleWithPermissionsDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
}