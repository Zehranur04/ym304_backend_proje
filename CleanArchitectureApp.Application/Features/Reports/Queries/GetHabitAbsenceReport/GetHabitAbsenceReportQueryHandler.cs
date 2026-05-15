using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitAbsenceReport;

public class GetHabitAbsenceReportQueryHandler : IRequestHandler<GetHabitAbsenceReportQuery, GetHabitAbsenceReportResponse>
{
    private readonly IApplicationDbContext _context;

    public GetHabitAbsenceReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetHabitAbsenceReportResponse> Handle(GetHabitAbsenceReportQuery request, CancellationToken cancellationToken)
    {
        var habits = await _context.Habits
            .Include(h => h.DailyRecords)
            .Where(h => h.UserId == request.UserId && h.IsDeleted == false && h.IsArchived == false)
            .ToListAsync(cancellationToken);

        var absences = new List<HabitAbsenceDto>();
        var today = DateTime.UtcNow.Date;

        foreach (var habit in habits)
        {
            // Start from habit creation date or at most 7 days ago if requested, 
            // but user said "oluşturulma tarihinden (InsertedAt veya varsayılan olarak son 7 gün)"
            // I'll use InsertedAt.Date if it's within 7 days, else 7 days ago, or just InsertedAt.Date.
            // Let's use InsertedAt.Date as the prompt suggested "oluşturulma tarihinden (InsertedAt veya varsayılan olarak son 7 gün) bugüne kadar"
            var startDate = habit.InsertedAt.Date;
            var sevenDaysAgo = today.AddDays(-7);
            
            // To be safe and practical (don't loop 3 years), let's take max of InsertedAt or 7 days ago, or just use InsertedAt.
            // The prompt says "varsayılan olarak son 7 gün", so let's default to last 7 days if InsertedAt is older.
            if (startDate < sevenDaysAgo)
            {
                startDate = sevenDaysAgo;
            }

            for (var loopDate = startDate; loopDate <= today; loopDate = loopDate.AddDays(1))
            {
                var record = habit.DailyRecords.FirstOrDefault(r => r.RecordDate.Date == loopDate);
                
                if (record == null || !record.IsCompletedForDay)
                {
                    absences.Add(new HabitAbsenceDto
                    {
                        HabitId = habit.Id,
                        HabitTitle = habit.Title,
                        Date = loopDate
                    });
                }
            }
        }

        return new GetHabitAbsenceReportResponse
        {
            Success = true,
            Message = "Devamsızlık raporu başarıyla getirildi.",
            Data = absences.OrderByDescending(a => a.Date).ToList()
        };
    }
}
