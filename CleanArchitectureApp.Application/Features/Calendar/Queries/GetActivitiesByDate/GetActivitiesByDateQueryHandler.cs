using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByDate;

public class GetActivitiesByDateQueryHandler : IRequestHandler<GetActivitiesByDateQuery, GetActivitiesByDateResponse>
{
    private readonly IApplicationDbContext _context;

    public GetActivitiesByDateQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivitiesByDateResponse> Handle(GetActivitiesByDateQuery request, CancellationToken cancellationToken)
    {
        var result = new DailyActivityDetailDto();
        var queryDate = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);

        var habitTrackings = await _context.HabitTrackings
            .Include(t => t.Habit)
            .Where(t => t.UserId == request.UserId && t.RecordDate.Date == queryDate)
            .ToListAsync(cancellationToken);

        foreach (var tracking in habitTrackings)
        {
            result.Activities.Add(new CalendarEventDto
            {
                Title = tracking.Habit.Title,
                Type = "Habit",
                Status = tracking.IsCompletedForDay ? ActivityStatus.Completed.ToString() : ActivityStatus.Ongoing.ToString(),
                ProgressValue = tracking.ProgressValue,
                TargetValue = tracking.Habit.TargetValue ?? 0
            });
        }

        var goalTrackings = await _context.GoalTrackings
            .Include(t => t.Goal)
            .Where(t => t.UserId == request.UserId && t.RecordDate.Date == queryDate)
            .ToListAsync(cancellationToken);

        foreach (var tracking in goalTrackings)
        {
            result.Activities.Add(new CalendarEventDto
            {
                Title = tracking.Goal.Title,
                Type = "Goal",
                Status = tracking.IsCompletedForInterval ? ActivityStatus.Completed.ToString() : ActivityStatus.Ongoing.ToString(),
                ProgressValue = tracking.ProgressValue,
                TargetValue = tracking.Goal.TargetValue ?? 0
            });
        }

        var futureGoals = await _context.Goals
            .Where(g => g.UserId == request.UserId && g.IsDeleted == false && g.TargetDate.Date == queryDate)
            .ToListAsync(cancellationToken);

        foreach (var goal in futureGoals)
        {
            result.Activities.Add(new CalendarEventDto
            {
                Title = goal.Title,
                Type = "Goal",
                Status = ActivityStatus.FuturePlan.ToString(),
                ProgressValue = goal.CurrentValue ?? 0,
                TargetValue = goal.TargetValue ?? 0
            });
        }

        return new GetActivitiesByDateResponse
        {
            Success = true,
            Message = "Günlük aktiviteler başarıyla getirildi.",
            Data = result
        };
    }
}
