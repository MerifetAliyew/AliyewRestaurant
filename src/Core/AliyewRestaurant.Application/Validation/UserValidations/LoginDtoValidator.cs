using AliyewRestaurant.Application.DTOs.UserDTOs;
using FluentValidation;

namespace AliyewRestaurant.Application.Validation.UserValidations;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz.")
            .EmailAddress().WithMessage("Düzgün email formatı daxil edin.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(6).WithMessage("Şifrə ən az 6 simvoldan ibarət olmalıdır.");
    }
}
