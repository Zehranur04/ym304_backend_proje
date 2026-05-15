using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.RemoveHabitProgress;

public class RemoveHabitProgressCommandHandler : IRequestHandler<RemoveHabitProgressCommand, RemoveHabitProgressResponse>
{
    private readonly IApplicationDbContext _context;

    public RemoveHabitProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RemoveHabitProgressResponse> Handle(RemoveHabitProgressCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var habit = await _context.Habits
            .Include(h => h.DailyRecords)
            .FirstOrDefaultAsync(h => h.Id == request.HabitId && h.UserId == request.UserId, cancellationToken);

        if (habit == null)
        {
            return new RemoveHabitProgressResponse
            {
                Success = false,
                Message = "Alışkanlık bulunamadı veya size ait değil."
            };
        }

        var todaysRecords = habit.DailyRecords
            .Where(r => r.RecordDate.Date == today)
            .OrderByDescending(r => r.Id)
            .ToList();

        var lastRecord = todaysRecords.FirstOrDefault();

        if (lastRecord == null)
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("HabitId", "Bugün için geri alınabilecek bir ilerleme kaydı bulunamadı.")
            };
            throw new ValidationException(failures);
        }

        var requestValue = lastRecord.ProgressValue;
        _context.HabitTrackings.Remove(lastRecord);

        // Remove the record from memory list to calculate remaining
        todaysRecords.Remove(lastRecord);

        var remainingProgress = todaysRecords.Sum(r => r.ProgressValue);

        var wasCompleted = habit.TargetValue.HasValue && (remainingProgress + requestValue) >= habit.TargetValue.Value;

        if (habit.TargetValue.HasValue && remainingProgress < habit.TargetValue.Value)
        {
            foreach (var record in todaysRecords)
            {
                record.IsCompletedForDay = false;
            }

            if (wasCompleted)
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
                if (userProfile != null)
                {
                    userProfile.TotalScore -= 10;
                    if (userProfile.TotalScore < 0) userProfile.TotalScore = 0;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new RemoveHabitProgressResponse
        {
            Success = true,
            Message = "Son ilerleme kaydı başarıyla geri alındı."
        };
    }
}
