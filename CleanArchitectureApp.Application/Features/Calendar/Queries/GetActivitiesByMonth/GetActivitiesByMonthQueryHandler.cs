using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByMonth;

public class GetActivitiesByMonthQueryHandler : IRequestHandler<GetActivitiesByMonthQuery, GetActivitiesByMonthResponse>
{
    private readonly IApplicationDbContext _context;

    public GetActivitiesByMonthQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivitiesByMonthResponse> Handle(GetActivitiesByMonthQuery request, CancellationToken cancellationToken)
    {
        var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);
        var summaries = new List<CalendarDaySummaryDto>();

        var habitTrackings = await _context.HabitTrackings
            .Where(t => t.UserId == request.UserId && t.RecordDate.Year == request.Year && t.RecordDate.Month == request.Month)
            .ToListAsync(cancellationToken);

        var goalTrackings = await _context.GoalTrackings
            .Where(t => t.UserId == request.UserId && t.RecordDate.Year == request.Year && t.RecordDate.Month == request.Month)
            .ToListAsync(cancellationToken);

        var goals = await _context.Goals
            .Where(g => g.UserId == request.UserId && g.IsDeleted == false && g.TargetDate.Year == request.Year && g.TargetDate.Month == request.Month)
            .ToListAsync(cancellationToken);

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(request.Year, request.Month, day);
            
            bool hasHabitTracking = habitTrackings.Any(t => t.RecordDate.Date == date.Date);
            bool hasGoalTracking = goalTrackings.Any(t => t.RecordDate.Date == date.Date);
            bool hasGoalTarget = goals.Any(g => g.TargetDate.Date == date.Date);

            summaries.Add(new CalendarDaySummaryDto
            {
                Date = date,
                HasActivity = hasHabitTracking || hasGoalTracking || hasGoalTarget
            });
        }

        return new GetActivitiesByMonthResponse
        {
            Success = true,
            Message = "Aylık aktiviteler başarıyla getirildi.",
            Data = summaries
        };
    }
}
