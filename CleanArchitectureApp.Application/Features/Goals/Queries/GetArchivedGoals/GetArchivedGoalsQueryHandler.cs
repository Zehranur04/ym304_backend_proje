using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetArchivedGoals;

public class GetArchivedGoalsQueryHandler : IRequestHandler<GetArchivedGoalsQuery, GetArchivedGoalsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetArchivedGoalsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetArchivedGoalsResponse> Handle(GetArchivedGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _context.Goals
            .Include(g => g.Category)
            .Where(g => g.UserId == request.UserId && g.IsArchived)
            .Select(g => new GoalListDto
            {
                Id = g.Id,
                Title = g.Title,
                CategoryName = g.Category.Name,
                TargetValue = g.TargetValue,
                CurrentValue = g.CurrentValue,
                Unit = g.Unit,
                Frequency = g.Frequency,
                IsCompleted = g.IsCompleted,
                IsArchived = g.IsArchived
            })
            .ToListAsync(cancellationToken);

        return new GetArchivedGoalsResponse
        {
            Success = true,
            Message = "Archived goals retrieved successfully.",
            Data = goals
        };
    }
}
