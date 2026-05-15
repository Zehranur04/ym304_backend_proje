using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Profile.Queries.GetUserProfile;

public class GetUserProfileResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserProfileDto? Data { get; set; }
}
