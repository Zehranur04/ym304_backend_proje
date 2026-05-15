using MediatR;

namespace CleanArchitectureApp.Application.Features.Auth.Login;

public class LoginRequest : IRequest<LoginResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
