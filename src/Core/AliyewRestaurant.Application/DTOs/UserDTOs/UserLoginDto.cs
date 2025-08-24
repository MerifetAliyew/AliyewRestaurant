namespace AliyewRestaurant.Application.DTOs.UserDTOs;

public record class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
