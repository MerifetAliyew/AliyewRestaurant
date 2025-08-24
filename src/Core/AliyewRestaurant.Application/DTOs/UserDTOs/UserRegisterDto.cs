namespace AliyewRestaurant.Application.DTOs.UserDTOs;

public record class UserRegisterDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
