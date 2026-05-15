using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Auth.Register;

public class RegisterValidation : AbstractValidator<RegisterRequest>
{
    public RegisterValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
    }
}
