using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
{
    private readonly IApplicationDbContext _context;

    public RegisterHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            Name = request.Name,
            Surname = request.Surname,
            Age = request.Age,
            Gender = request.Gender,
            UserProfile = new UserProfile
            {
                Bio = "Merhaba, hedeflerime ulaşmak için buradayım!",
                ProfilePictureUrl = ""
            }
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse
        {
            Id = user.Id,
            Message = "Kullanıcı kaydı başarıyla oluşturuldu."
        };
    }
}
