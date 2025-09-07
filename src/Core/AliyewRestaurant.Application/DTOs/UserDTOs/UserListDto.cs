namespace AliyewRestaurant.Application.DTOs.UserDTOs;

public class UserListDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}