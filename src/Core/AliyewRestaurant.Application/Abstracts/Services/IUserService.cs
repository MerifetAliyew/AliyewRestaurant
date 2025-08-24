using AliyewRestaurant.Application.DTOs.UserDTOs;
using AliyewRestaurant.Application.Shared;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IUserService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
    Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}
