using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetGoalStatistics;

public class GetGoalStatisticsQueryHandler : IRequestHandler<GetGoalStatisticsQuery, GetGoalStatisticsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetGoalStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetGoalStatisticsResponse> Handle(GetGoalStatisticsQuery request, CancellationToken cancellationToken)
    {
        var completed = await _context.Goals.CountAsync(g => g.UserId == request.UserId && g.IsCompleted == true && g.IsDeleted == false, cancellationToken);
        var active = await _context.Goals.CountAsync(g => g.UserId == request.UserId && g.IsCompleted == false && g.IsDeleted == false, cancellationToken);

        var total = completed + active;
        double rate = total > 0 ? Math.Round((double)completed / total * 100, 1) : 0;

        var dto = new GoalStatisticsDto
        {
            CompletedGoalsCount = completed,
            ActiveGoalsCount = active,
            SuccessRate = rate
        };

        return new GetGoalStatisticsResponse
        {
            Success = true,
            Message = "Hedef istatistikleri getirildi.",
            Data = dto
        };
    }
}
