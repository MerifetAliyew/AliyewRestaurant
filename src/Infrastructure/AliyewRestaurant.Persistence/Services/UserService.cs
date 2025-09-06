using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.UserDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Application.Shared.Settings;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AliyewRestaurant.Persistence.Services;

public class UserService : IUserService
{
    private UserManager<AppUser> _userManager { get; }
    private SignInManager<AppUser> _signInManager { get; }
    private JWTSettings _jwtSetting { get; }
    private readonly IEmailService _emailService;
    private RoleManager<IdentityRole> _roleManager { get; }
    public UserService(UserManager<AppUser> userManager,
           SignInManager<AppUser> signInManager,
           IOptions<JWTSettings> jwtSetting,
           RoleManager<IdentityRole> roleManager,
           IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSetting = jwtSetting.Value;
        _roleManager = roleManager;
        _emailService = emailService;
    }
    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
        var existedEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existedEmail is not null)
        {
            return new BaseResponse<string>(
                "Bu e-poçt artıq mövcuddur",
                false,
                HttpStatusCode.BadRequest
            );
        }

        AppUser newUser = new()
        {
            Email = dto.Email,
            FullName = dto.FullName,
            UserName = dto.Email
        };

        IdentityResult identityResult = await _userManager.CreateAsync(newUser, dto.Password);

        if (!identityResult.Succeeded)
        {
            StringBuilder errorsMessage = new();
            foreach (var error in identityResult.Errors)
            {
                errorsMessage.Append(error.Description + "; ");
            }

            return new BaseResponse<string>(
                errorsMessage.ToString(),
                false,
                HttpStatusCode.BadRequest
            );
        }
        string confirmLink = await GetEmailConfirmLink(newUser);
        await _emailService.SendEmailAsync(
            new List<string> { newUser.Email }, // email-i siyahıya çevir
            "Hesabınızı təsdiq edin",
            $"Salam {newUser.FullName}! Hesabınızı təsdiqləmək üçün bu linkə klikləyin: {confirmLink}"
        );
        return new BaseResponse<string>(
            "Hesab uğurla yaradıldı. Zəhmət olmasa emailinizi yoxlayın və hesabınızı təsdiqləyin.",
            true,
            HttpStatusCode.Created
        );
    }

    public async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
    {

        var existedEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existedEmail is null)
        {
            return new("Email or password is wrong.", null, System.Net.HttpStatusCode.NotFound);
        }

        if (!existedEmail.EmailConfirmed)
        {
            return new("Please confirm your email", null, System.Net.HttpStatusCode.BadRequest);
        }

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

        if (!signInResult.Succeeded)
        {
            return new("Email or password is wrong.", null, System.Net.HttpStatusCode.NotFound);
        }
        var token = await GenerateTokensAsync(existedEmail);
        return new("Token generated", token, System.Net.HttpStatusCode.OK);
    }

    private async Task<TokenResponse> GenerateTokensAsync(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));

            // Hər rol üçün permission-ları əlavə et
            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
                foreach (var claim in roleClaims.Where(c => c.Type == "Permission"))
                {
                    claims.Add(claim);
                }
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinutes),
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2); //  7 day  
        user.RefreshToken = refreshToken;
        user.ExpiryDate = refreshTokenExpiryDate;
        await _userManager.UpdateAsync(user);

        return new TokenResponse
        {
            Token = jwt,
            RefreshToken = refreshToken,
            ExpireDate = tokenDescriptor.Expires!.Value
        };
    }

    public async Task<BaseResponse<string>> ConfirmEmail(string userId, string token)
    {
        var existedUser = await _userManager.FindByIdAsync(userId);
        if (existedUser is null)
        {
            return new BaseResponse<string>("Email confirmation failed.", HttpStatusCode.NotFound);
        }

        var result = await _userManager.ConfirmEmailAsync(existedUser, token);
        if (!result.Succeeded)
        {
            return new BaseResponse<string>("Email confirmation failed.", HttpStatusCode.BadRequest);
        }

        return new BaseResponse<string>("Email confirmed successfully.", HttpStatusCode.OK);
    }

    private async Task<string> GetEmailConfirmLink(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = $"https://localhost:7232/api/Accounts/confirm-email?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
        Console.WriteLine(token);
        return link;

    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            return new("Invalid access token", null, HttpStatusCode.Unauthorized);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
            return new("User not found", null, HttpStatusCode.NotFound);

        if (user.RefreshToken is null || user.RefreshToken != request.RefreshToken ||
            user.ExpiryDate < DateTime.UtcNow)
            return new("Invalid refresh token", null, HttpStatusCode.BadRequest);

        var newAccessToken = await GenerateTokensAsync(user);
        return new("Token refreshed", newAccessToken, HttpStatusCode.OK);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, // Token-in vaxtını yoxlama
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<BaseResponse<string>> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new BaseResponse<string>("İstifadəçi tapılmadı.", HttpStatusCode.NotFound);

        // 2️⃣ Şifrə sıfırlama token-i yarat
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // 3️⃣ Reset linki yarat (token URL-encode olunur)
        var resetLink = $"https://localhost:7232/api/accounts/reset-password?email={user.Email}&token={HttpUtility.UrlEncode(token)}";

        // 4️⃣ Email göndərməyi try-catch ilə qoruma
        try
        {
            await _emailService.SendEmailAsync(
                new[] { user.Email }, // string-i array şəklində veririk
                "Şifrənizi sıfırlayın",
                $"Salam {user.FullName}! Şifrənizi sıfırlamaq üçün bu linkə klikləyin: <a href='{resetLink}'>Klik et</a>"
            );
        }
        catch (Exception ex)
        {
            // Email göndərilməsə də register/metod uğurlu sayılır
            Console.WriteLine("Email göndərmə xətası: " + ex.Message);
        }

        // 5️⃣ Uğurlu cavab qaytar
        return new BaseResponse<string>(
            "Şifrə sıfırlama linki emailinizə göndərildi.",
            true,
            HttpStatusCode.OK
        );
    }

    public async Task<BaseResponse<string>> ResetPasswordAsync(UserResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return new BaseResponse<string>("User not found.", HttpStatusCode.NotFound);

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string>($"Password reset failed: {errors}", HttpStatusCode.BadRequest);
        }

        return new BaseResponse<string>("Password has been reset successfully.", HttpStatusCode.OK);
    }
}
