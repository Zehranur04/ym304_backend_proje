using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.RemoveGoalProgress;

public class RemoveGoalProgressCommandHandler : IRequestHandler<RemoveGoalProgressCommand, RemoveGoalProgressResponse>
{
    private readonly IApplicationDbContext _context;

    public RemoveGoalProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RemoveGoalProgressResponse> Handle(RemoveGoalProgressCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .Include(g => g.TrackingRecords)
            .FirstOrDefaultAsync(g => g.Id == request.GoalId && g.UserId == request.UserId, cancellationToken);

        if (goal == null)
        {
            return new RemoveGoalProgressResponse
            {
                Success = false,
                Message = "Hedef bulunamadı veya size ait değil."
            };
        }

        var trackingRecords = goal.TrackingRecords.OrderByDescending(r => r.Id).ToList();
        var lastRecord = trackingRecords.FirstOrDefault();

        if (lastRecord == null)
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("GoalId", "Geri alınabilecek bir hedef ilerleme kaydı bulunamadı.")
            };
            throw new ValidationException(failures);
        }

        var requestValue = lastRecord.ProgressValue;
        _context.GoalTrackings.Remove(lastRecord);
        trackingRecords.Remove(lastRecord);

        var newTotalProgress = trackingRecords.Sum(r => r.ProgressValue);
        goal.CurrentValue = newTotalProgress;

        var wasCompleted = goal.TargetValue.HasValue && (newTotalProgress + requestValue) >= goal.TargetValue.Value;

        if (goal.TargetValue.HasValue && newTotalProgress < goal.TargetValue.Value)
        {
            goal.IsCompleted = false;

            foreach (var record in trackingRecords)
            {
                record.IsCompletedForInterval = false;
            }

            if (wasCompleted)
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
                if (userProfile != null)
                {
                    userProfile.TotalScore -= 50;
                    if (userProfile.TotalScore < 0) userProfile.TotalScore = 0;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new RemoveGoalProgressResponse
        {
            Success = true,
            Message = "Son hedef ilerleme kaydı başarıyla geri alındı."
        };
    }
}
