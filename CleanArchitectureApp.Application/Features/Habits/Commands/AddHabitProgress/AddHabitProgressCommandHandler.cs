using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Enums;
using CleanArchitectureApp.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.AddHabitProgress;

public class AddHabitProgressCommandHandler : IRequestHandler<AddHabitProgressCommand, AddHabitProgressResponse>
{
    private readonly IApplicationDbContext _context;

    public AddHabitProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AddHabitProgressResponse> Handle(AddHabitProgressCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

        var habit = await _context.Habits
            .Include(h => h.DailyRecords)
            .FirstOrDefaultAsync(h => h.Id == request.HabitId && h.UserId == request.UserId, cancellationToken);

        if (habit == null)
        {
            return new AddHabitProgressResponse
            {
                Success = false,
                Message = "Alışkanlık bulunamadı veya size ait değil.",
                Data = 0
            };
        }

        var today = DateTime.UtcNow.Date;

        var tracking = new HabitTracking
        {
            HabitId = habit.Id,
            UserId = request.UserId,
            ProgressValue = request.Value,
            RecordDate = today,
            InsertedAt = DateTime.UtcNow,
            IsCompletedForDay = false
        };

        var previousTotal = habit.DailyRecords
            .Where(r => r.RecordDate.Date == today)
            .Sum(r => r.ProgressValue);

        var newTotal = previousTotal + request.Value;

        int percentage = habit.TargetValue.HasValue && habit.TargetValue.Value > 0 ? (int)Math.Min(100, Math.Round((double)newTotal / (double)habit.TargetValue.Value * 100)) : 0;

        var wasCompleted = habit.TargetValue.HasValue && previousTotal >= habit.TargetValue.Value;
        var isCompletedNow = habit.TargetValue.HasValue && newTotal >= habit.TargetValue.Value;

        // Haftalık basit alışkanlıklarda hedef yoksa her kayıt = bu haftayı tamamla
        if (!habit.TargetValue.HasValue && habit.Frequency == FrequencyType.Weekly)
            isCompletedNow = true;

        if (isCompletedNow)
        {
            tracking.IsCompletedForDay = true;

            var todaysAllRecords = await _context.HabitTrackings
                .Where(t => t.HabitId == habit.Id && t.RecordDate.Date == today)
                .ToListAsync(cancellationToken);

            foreach (var record in todaysAllRecords)
            {
                record.IsCompletedForDay = true;
            }
        }

        if (!wasCompleted && isCompletedNow)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (userProfile != null)
            {
                userProfile.TotalScore += 10;
            }
            else
            {
                var newProfile = new UserProfile { UserId = request.UserId, TotalScore = 10 };
                _context.UserProfiles.Add(newProfile);
            }
        }

        _context.HabitTrackings.Add(tracking);
        await _context.SaveChangesAsync(cancellationToken);

        var newBadge = await CheckAndAwardBadgesAsync(habit.Id, request.UserId, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new AddHabitProgressResponse
        {
            Success = true,
            Message = "İlerleme kaydedildi.",
            Data = tracking.Id,
            CurrentAmount = (decimal)newTotal,
            Percentage = percentage,
            HasNewBadge = newBadge != null,
            NewBadgeName = newBadge?.BadgeName,
            NewBadgeDescription = newBadge?.Description,
            NewBadgeIcon = newBadge?.IconData
        };
    }

    private async Task<CleanArchitectureApp.Domain.Entities.UserBadge?> CheckAndAwardBadgesAsync(int habitId, int userId, CancellationToken cancellationToken)
    {
        var pastLogs = await _context.HabitTrackings
            .Where(t => t.HabitId == habitId && t.IsCompletedForDay == true)
            .Select(t => t.RecordDate.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync(cancellationToken);

        int streakCount = 0;
        DateTime checkDate = DateTime.UtcNow.Date;

        foreach (var date in pastLogs)
        {
            if (date == checkDate)
            {
                streakCount++;
                checkDate = checkDate.AddDays(-1);
            }
            else
            {
                break;
            }
        }

        bool has30DayBadge    = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "30 Günlük Demir",   cancellationToken);
        bool hasSevenDayBadge  = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "7 Günlük İstikrar", cancellationToken);
        bool hasThreeDayBadge  = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "3 Gün Ateşi",      cancellationToken);

        CleanArchitectureApp.Domain.Entities.UserBadge? earnedBadge = null;

        // En değerliden başlayarak kontrol et; her seferinde yalnızca bir rozet kazandır
        if (streakCount >= 30 && !has30DayBadge)
        {
            earnedBadge = new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId,
                BadgeName = "30 Günlük Demir",
                Description = "30 gün kesintisiz! Sen artık bir alışkanlık makinesisin.",
                IconData = "🔩",
                EarnedAt = DateTime.UtcNow
            };
            _context.UserBadges.Add(earnedBadge);
        }
        else if (streakCount >= 7 && !hasSevenDayBadge)
        {
            earnedBadge = new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId,
                BadgeName = "7 Günlük İstikrar",
                Description = "Tam bir hafta! İradene hayran kaldık.",
                IconData = "⭐",
                EarnedAt = DateTime.UtcNow
            };
            _context.UserBadges.Add(earnedBadge);
        }
        else if (streakCount >= 3 && !hasThreeDayBadge)
        {
            earnedBadge = new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId,
                BadgeName = "3 Gün Ateşi",
                Description = "Harika başlangıç! 3 gün üst üste görevini tamamladın.",
                IconData = "🔥",
                EarnedAt = DateTime.UtcNow
            };
            _context.UserBadges.Add(earnedBadge);
        }

        if (earnedBadge != null)
        {
            _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
            {
                UserId = userId,
                Title = $"{earnedBadge.IconData} Yeni Rozet Kazandın!",
                Message = $"'{earnedBadge.BadgeName}' rozetini kazandın! {earnedBadge.Description}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        return earnedBadge;
    }
}
