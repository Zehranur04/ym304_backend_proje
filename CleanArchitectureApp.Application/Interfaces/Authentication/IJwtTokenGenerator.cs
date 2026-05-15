namespace CleanArchitectureApp.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId);
}
