using MediatR;

namespace CleanArchitectureApp.Application.Features.Profile.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<GetUserProfileResponse>
{
    public int UserId { get; set; }
}
