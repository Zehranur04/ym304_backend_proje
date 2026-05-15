using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CompleteHabit;

public class CompleteHabitCommandHandler : IRequestHandler<CompleteHabitCommand, CompleteHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public CompleteHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompleteHabitResponse> Handle(CompleteHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (habit == null)
        {
            return new CompleteHabitResponse
            {
                Success = false,
                Message = "Alışkanlık bulunamadı."
            };
        }

        habit.IsCompleted = true;
        habit.CompletedAt = DateTime.UtcNow;
        habit.UpdatedAt = DateTime.UtcNow;

        // Tamamlama puanı: alışkanlık ilk kez tamamlanıyorsa +20 puan
        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == habit.UserId, cancellationToken);
        if (userProfile != null)
            userProfile.TotalScore += 20;
        else
            _context.UserProfiles.Add(new CleanArchitectureApp.Domain.Entities.UserProfile
                { UserId = habit.UserId, TotalScore = 20 });

        await _context.SaveChangesAsync(cancellationToken);

        try { await CheckAndAwardBadgesAsync(habit.UserId, cancellationToken); }
        catch { /* rozet hatası tamamlamayı engellemez */ }

        return new CompleteHabitResponse
        {
            Success = true,
            Message = "Alışkanlık başarıyla tamamlandı."
        };
    }

    private async Task CheckAndAwardBadgesAsync(int userId, CancellationToken cancellationToken)
    {
        // Tamamlanan alışkanlık sayısına göre milestone rozetleri
        int completedCount = await _context.Habits
            .CountAsync(h => h.UserId == userId && h.IsCompleted, cancellationToken);

        var milestones = new[]
        {
            (count: 1,  name: "İlk Adım",        desc: "İlk alışkanlığını tamamladın. Her büyük yolculuk bir adımla başlar!", icon: "🌱"),
            (count: 5,  name: "Alışkanlık Ustası", desc: "5 alışkanlığını tamamladın. Kararlılığın ilham verici!",             icon: "🏅"),
            (count: 10, name: "Efsane",            desc: "10 alışkanlık! Sen artık gerçek bir şampiyon efsanesisin.",          icon: "👑"),
        };

        bool anyAdded = false;
        foreach (var m in milestones)
        {
            if (completedCount >= m.count)
            {
                bool alreadyHas = await _context.UserBadges
                    .AnyAsync(b => b.UserId == userId && b.BadgeName == m.name, cancellationToken);

                if (!alreadyHas)
                {
                    _context.UserBadges.Add(new CleanArchitectureApp.Domain.Entities.UserBadge
                    {
                        UserId = userId,
                        BadgeName = m.name,
                        Description = m.desc,
                        IconData = m.icon,
                        EarnedAt = DateTime.UtcNow
                    });
                    _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
                    {
                        UserId = userId,
                        Title = $"{m.icon} Yeni Rozet Kazandın!",
                        Message = $"'{m.name}' rozetini kazandın! {m.desc}",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                    anyAdded = true;
                }
            }
        }

        // Streak rozeti (mevcut mantık korundu, alışkanlık tamamlandığında da çalışsın)
        var pastLogs = await _context.HabitTrackings
            .Where(t => t.UserId == userId && t.IsCompletedForDay == true)
            .Select(t => t.RecordDate.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync(cancellationToken);

        int streakCount = 0;
        DateTime checkDate = DateTime.UtcNow.Date;
        foreach (var date in pastLogs)
        {
            if (date == checkDate) { streakCount++; checkDate = checkDate.AddDays(-1); }
            else break;
        }

        bool hasThreeDayBadge  = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "3 Gün Ateşi",      cancellationToken);
        bool hasSevenDayBadge  = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "7 Günlük İstikrar", cancellationToken);
        bool has30DayBadge     = await _context.UserBadges.AnyAsync(b => b.UserId == userId && b.BadgeName == "30 Günlük Demir",   cancellationToken);

        if (streakCount >= 30 && !has30DayBadge)
        {
            _context.UserBadges.Add(new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId, BadgeName = "30 Günlük Demir",
                Description = "30 gün kesintisiz! Sen artık bir alışkanlık makinesisin.",
                IconData = "🔩", EarnedAt = DateTime.UtcNow
            });
            _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
            {
                UserId = userId, Title = "🔩 Yeni Rozet Kazandın!",
                Message = "'30 Günlük Demir' rozetini kazandın! 30 gün kesintisiz, artık bir alışkanlık makinesisin.",
                IsRead = false, CreatedAt = DateTime.UtcNow
            });
            anyAdded = true;
        }
        if (streakCount >= 7 && !hasSevenDayBadge)
        {
            _context.UserBadges.Add(new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId, BadgeName = "7 Günlük İstikrar",
                Description = "Tam bir hafta! İradene hayran kaldık.",
                IconData = "⭐", EarnedAt = DateTime.UtcNow
            });
            _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
            {
                UserId = userId, Title = "⭐ Yeni Rozet Kazandın!",
                Message = "'7 Günlük İstikrar' rozetini kazandın! Tam bir hafta, iradene hayran kaldık.",
                IsRead = false, CreatedAt = DateTime.UtcNow
            });
            anyAdded = true;
        }
        if (streakCount >= 3 && !hasThreeDayBadge)
        {
            _context.UserBadges.Add(new CleanArchitectureApp.Domain.Entities.UserBadge
            {
                UserId = userId, BadgeName = "3 Gün Ateşi",
                Description = "Harika başlangıç! 3 gün üst üste görevini tamamladın.",
                IconData = "🔥", EarnedAt = DateTime.UtcNow
            });
            _context.Notifications.Add(new CleanArchitectureApp.Domain.Entities.Notification
            {
                UserId = userId, Title = "🔥 Yeni Rozet Kazandın!",
                Message = "'3 Gün Ateşi' rozetini kazandın! Harika başlangıç, 3 gün üst üste tamamladın.",
                IsRead = false, CreatedAt = DateTime.UtcNow
            });
            anyAdded = true;
        }

        if (anyAdded)
            await _context.SaveChangesAsync(cancellationToken);
    }
}
