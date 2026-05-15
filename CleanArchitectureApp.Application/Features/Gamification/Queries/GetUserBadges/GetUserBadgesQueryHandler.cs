using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Gamification.Queries.GetUserBadges;

public class GetUserBadgesQueryHandler : IRequestHandler<GetUserBadgesQuery, GetUserBadgesResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserBadgesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserBadgesResponse> Handle(GetUserBadgesQuery request, CancellationToken cancellationToken)
    {
        var badges = await _context.UserBadges
            .Where(b => b.UserId == request.UserId)
            .OrderByDescending(b => b.EarnedAt)
            .Select(b => new UserBadgeDto
            {
                Id = b.Id,
                UserId = b.UserId,
                BadgeName = b.BadgeName,
                Description = b.Description,
                IconData = b.IconData,
                EarnedAt = b.EarnedAt
            })
            .ToListAsync(cancellationToken);

        return new GetUserBadgesResponse
        {
            Success = true,
            Message = "Kullanıcı rozetleri başarıyla getirildi.",
            Data = badges
        };
    }
}
