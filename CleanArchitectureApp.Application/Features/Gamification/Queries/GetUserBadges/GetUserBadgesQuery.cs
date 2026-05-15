using MediatR;

namespace CleanArchitectureApp.Application.Features.Gamification.Queries.GetUserBadges;

public class GetUserBadgesQuery : IRequest<GetUserBadgesResponse>
{
    public int UserId { get; set; }
}
