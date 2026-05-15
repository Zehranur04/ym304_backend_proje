using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitStatistics;

public class GetHabitStatisticsQueryHandler : IRequestHandler<GetHabitStatisticsQuery, GetHabitStatisticsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetHabitStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetHabitStatisticsResponse> Handle(GetHabitStatisticsQuery request, CancellationToken cancellationToken)
    {
        var completed = await _context.Habits.CountAsync(h => h.UserId == request.UserId && h.IsCompleted == true && h.IsDeleted == false, cancellationToken);
        var active = await _context.Habits.CountAsync(h => h.UserId == request.UserId && h.IsCompleted == false && h.IsDeleted == false && h.IsArchived == false, cancellationToken);

        var total = completed + active;
        double rate = total > 0 ? Math.Round((double)completed / total * 100, 1) : 0;

        var dto = new HabitStatisticsDto
        {
            CompletedHabitsCount = completed,
            ActiveHabitsCount = active,
            SuccessRate = rate
        };

        return new GetHabitStatisticsResponse
        {
            Success = true,
            Message = "Alışkanlık istatistikleri getirildi.",
            Data = dto
        };
    }
}
