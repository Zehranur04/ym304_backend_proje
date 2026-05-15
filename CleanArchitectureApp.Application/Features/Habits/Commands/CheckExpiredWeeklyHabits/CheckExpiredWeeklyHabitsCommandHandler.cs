using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CheckExpiredWeeklyHabits;

public class CheckExpiredWeeklyHabitsCommandHandler : IRequestHandler<CheckExpiredWeeklyHabitsCommand, CheckExpiredWeeklyHabitsResponse>
{
    private readonly IApplicationDbContext _context;

    public CheckExpiredWeeklyHabitsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CheckExpiredWeeklyHabitsResponse> Handle(CheckExpiredWeeklyHabitsCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        // Oluşturulma tarihinden itibaren 7 günü geçmiş, henüz tamamlanmamış haftalık alışkanlıklar
        var expired = await _context.Habits
            .Where(h => h.UserId == request.UserId
                     && h.Frequency == FrequencyType.Weekly
                     && !h.IsCompleted
                     && !h.IsArchived
                     && h.StartDate.Date.AddDays(7) <= today)
            .ToListAsync(cancellationToken);

        if (!expired.Any())
            return new CheckExpiredWeeklyHabitsResponse { Success = true, Message = "Süresi dolmuş haftalık alışkanlık yok.", Data = 0 };

        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        foreach (var habit in expired)
        {
            habit.IsCompleted = true;
            habit.CompletedAt = DateTime.UtcNow;
            habit.UpdatedAt = DateTime.UtcNow;

            // 7 günlük pencere tamamlandığı için +20 puan
            if (userProfile != null)
                userProfile.TotalScore += 20;
            else
            {
                userProfile = new CleanArchitectureApp.Domain.Entities.UserProfile { UserId = request.UserId, TotalScore = 20 };
                _context.UserProfiles.Add(userProfile);
            }

            _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
            {
                UserId = request.UserId,
                Title = "🏁 Haftalık Alışkanlık Tamamlandı!",
                Message = $"'{habit.Title}' alışkanlığının 7 günlük süresi doldu ve tamamlandı. Tebrikler! 🎉",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new CheckExpiredWeeklyHabitsResponse
        {
            Success = true,
            Message = $"{expired.Count} haftalık alışkanlık otomatik tamamlandı.",
            Data = expired.Count
        };
    }
}
