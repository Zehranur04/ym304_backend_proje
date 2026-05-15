using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Profile.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<UpdateUserProfileResponse>
{
    [JsonIgnore]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
