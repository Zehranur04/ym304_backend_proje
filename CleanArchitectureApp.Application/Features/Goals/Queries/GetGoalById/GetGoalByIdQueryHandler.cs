using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalById;

public class GetGoalByIdQueryHandler : IRequestHandler<GetGoalByIdQuery, GetGoalByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetGoalByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetGoalByIdResponse> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .Include(g => g.Category)
            .Where(g => g.Id == request.Id)
            .Select(g => new GoalDetailDto
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                CategoryName = g.Category.Name,
                TargetValue = g.TargetValue,
                CurrentValue = g.CurrentValue,
                Unit = g.Unit,
                TargetDate = g.TargetDate,
                IsCompleted = g.IsCompleted,
                InsertedAt = g.InsertedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (goal == null)
        {
            return new GetGoalByIdResponse
            {
                Success = false,
                Message = "Hedef bulunamadı",
                Data = null
            };
        }

        return new GetGoalByIdResponse
        {
            Success = true,
            Message = "Goal retrieved successfully.",
            Data = goal
        };
    }
}
