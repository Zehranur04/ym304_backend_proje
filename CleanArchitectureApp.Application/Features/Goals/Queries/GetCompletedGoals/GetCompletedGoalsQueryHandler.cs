using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetCompletedGoals;

public class GetCompletedGoalsQueryHandler : IRequestHandler<GetCompletedGoalsQuery, GetCompletedGoalsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetCompletedGoalsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCompletedGoalsResponse> Handle(GetCompletedGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _context.Goals
            .Include(g => g.Category)
            .Where(g => g.UserId == request.UserId && g.IsCompleted == true && g.IsDeleted == false && g.IsArchived == false)
            .Select(g => new GoalListDto
            {
                Id = g.Id,
                Title = g.Title,
                CategoryName = g.Category.Name,
                TargetValue = g.TargetValue,
                CurrentValue = g.CurrentValue,
                Unit = g.Unit,
                IsCompleted = g.IsCompleted
            })
            .ToListAsync(cancellationToken);

        return new GetCompletedGoalsResponse
        {
            Success = true,
            Message = "Completed goals retrieved successfully.",
            Data = goals
        };
    }
}
