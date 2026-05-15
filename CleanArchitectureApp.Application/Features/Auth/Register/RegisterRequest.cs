using MediatR;
using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Application.Features.Auth.Register;

public class RegisterRequest : IRequest<RegisterResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public Gender Gender { get; set; }
}
