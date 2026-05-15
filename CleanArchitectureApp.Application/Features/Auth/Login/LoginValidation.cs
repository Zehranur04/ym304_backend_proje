using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Auth.Login;

public class LoginValidation : AbstractValidator<LoginRequest>
{
    public LoginValidation()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
