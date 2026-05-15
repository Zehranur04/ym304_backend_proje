using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CompleteGoal;

public class CompleteGoalCommandHandler : IRequestHandler<CompleteGoalCommand, CompleteGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public CompleteGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompleteGoalResponse> Handle(CompleteGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (goal == null)
        {
            return new CompleteGoalResponse
            {
                Success = false,
                Message = "Hedef bulunamadı."
            };
        }

        goal.IsCompleted = true;
        goal.CompletedAt = DateTime.UtcNow;
        goal.UpdatedAt = DateTime.UtcNow;

        // Tamamlama puanı: hedef tamamlandığında +50 puan
        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == goal.UserId, cancellationToken);
        if (userProfile != null)
            userProfile.TotalScore += 50;
        else
            _context.UserProfiles.Add(new CleanArchitectureApp.Domain.Entities.UserProfile
                { UserId = goal.UserId, TotalScore = 50 });

        await _context.SaveChangesAsync(cancellationToken);

        try { await CheckAndAwardBadgesAsync(goal.UserId, cancellationToken); }
        catch { /* rozet hatası hedef tamamlamayı engellemez */ }

        return new CompleteGoalResponse
        {
            Success = true,
            Message = "Hedef başarıyla tamamlandı."
        };
    }

    private async Task CheckAndAwardBadgesAsync(int userId, CancellationToken cancellationToken)
    {
        int completedCount = await _context.Goals
            .CountAsync(g => g.UserId == userId && g.IsCompleted, cancellationToken);

        var milestones = new[]
        {
            (count: 1,  name: "Hedef Avcısı", desc: "İlk hedefini tamamladın. Harika bir başlangıç!",              icon: "🎯"),
            (count: 5,  name: "Kararlı",       desc: "5 hedef tamamlandı. Odaklanma gücün gerçekten etkileyici!",  icon: "💪"),
            (count: 10, name: "Vizyon Sahibi", desc: "10 hedef! Hayallerini gerçeğe dönüştürme konusunda ustasın.", icon: "🌟"),
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

        if (anyAdded)
            await _context.SaveChangesAsync(cancellationToken);
    }
}
