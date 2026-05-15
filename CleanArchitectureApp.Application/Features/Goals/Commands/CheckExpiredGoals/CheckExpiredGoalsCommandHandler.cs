using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CheckExpiredGoals;

public class CheckExpiredGoalsCommandHandler : IRequestHandler<CheckExpiredGoalsCommand, CheckExpiredGoalsResponse>
{
    private readonly IApplicationDbContext _context;

    public CheckExpiredGoalsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CheckExpiredGoalsResponse> Handle(CheckExpiredGoalsCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var expiredGoals = await _context.Goals
            .Where(g => g.UserId == request.UserId 
                     && g.IsCompleted == false 
                     && g.IsDeleted == false 
                     && g.TargetDate.Date < today)
            .ToListAsync(cancellationToken);

        if (!expiredGoals.Any())
        {
            return new CheckExpiredGoalsResponse
            {
                Success = true,
                Message = "Süresi geçen hedef bulunamadı.",
                Data = 0
            };
        }

        var expiredCount = expiredGoals.Count;

        foreach (var goal in expiredGoals)
        {
            goal.IsDeleted = true; // Mark as failed/cancelled
            goal.UpdatedAt = DateTime.UtcNow;
        }

        var penalty = expiredCount * 50;

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
        
        if (userProfile != null)
        {
            userProfile.TotalScore -= penalty;
            if (userProfile.TotalScore < 0)
            {
                userProfile.TotalScore = 0;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new CheckExpiredGoalsResponse
        {
            Success = true,
            Message = $"Süresi geçen {expiredCount} hedef kontrol edildi ve puanlar güncellendi.",
            Data = expiredCount
        };
    }
}
