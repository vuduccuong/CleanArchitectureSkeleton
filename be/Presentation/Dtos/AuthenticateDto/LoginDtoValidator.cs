using FluentValidation;

namespace Presentation.Dtos.AuthenticateDto
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(v => v.LoginId).NotEmpty().NotNull();
            RuleFor(v => v.Password).NotEmpty().NotNull();
        }
    }
}
