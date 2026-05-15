using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetActiveHabits;

public class GetActiveHabitsQueryHandler : IRequestHandler<GetActiveHabitsQuery, GetActiveHabitsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetActiveHabitsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetActiveHabitsResponse> Handle(GetActiveHabitsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var habits = await _context.Habits
            .Include(h => h.Category)
            .Include(h => h.DailyRecords)
            .Where(h => h.UserId == request.UserId && !h.IsArchived && !h.IsCompleted && !h.IsDeleted)
            .ToListAsync(cancellationToken);

        var dtos = habits.Select(h =>
        {
            bool isWeekly = h.Frequency == FrequencyType.Weekly;
            decimal currentValue;
            bool completedThisPeriod;
            int? daysUntilAvailable = null;

            if (isWeekly)
            {
                // Pencere: oluşturulma tarihinden itibaren 7 gün
                var windowStart = h.StartDate.Date;
                var windowEnd = windowStart.AddDays(6); // dahil, son gün

                currentValue = h.DailyRecords
                    .Where(r => r.RecordDate.Date >= windowStart && r.RecordDate.Date <= today)
                    .Sum(r => r.ProgressValue);

                bool targetReached = h.TargetValue.HasValue && h.TargetValue.Value > 0
                    && currentValue >= h.TargetValue.Value;

                // Bugün zaten işaretlendi mi? (basit tip çift tıklama engeli)
                bool completedToday = h.DailyRecords
                    .Any(r => r.RecordDate.Date == today && r.IsCompletedForDay);

                completedThisPeriod = targetReached || completedToday;

                // Pencerede kaç gün kaldı (0 = son gün, negatif = süresi dolmuş ama henüz temizlenmemiş)
                var remaining = (int)(windowEnd - today).TotalDays;
                daysUntilAvailable = remaining >= 0 ? remaining : 0;
            }
            else
            {
                currentValue = h.DailyRecords
                    .Where(r => r.RecordDate.Date == today)
                    .Sum(r => r.ProgressValue);

                completedThisPeriod = h.DailyRecords
                    .Any(r => r.RecordDate.Date == today && r.IsCompletedForDay);
            }

            return new HabitListDto
            {
                Id = h.Id,
                Title = h.Title,
                CategoryName = h.Category.Name,
                TargetValue = h.TargetValue,
                Unit = h.Unit,
                TrackingType = h.TrackingType,
                Frequency = h.Frequency,
                CurrentValue = currentValue,
                CompletedThisPeriod = completedThisPeriod,
                DaysUntilAvailable = daysUntilAvailable
            };
        }).ToList();

        return new GetActiveHabitsResponse
        {
            Success = true,
            Message = "Active habits retrieved successfully.",
            Data = dtos
        };
    }
}
