using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.AddGoalProgress;

public class AddGoalProgressCommandHandler : IRequestHandler<AddGoalProgressCommand, AddGoalProgressResponse>
{
    private readonly IApplicationDbContext _context;

    public AddGoalProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AddGoalProgressResponse> Handle(AddGoalProgressCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .Include(g => g.TrackingRecords)
            .FirstOrDefaultAsync(g => g.Id == request.GoalId && g.UserId == request.UserId, cancellationToken);

        if (goal == null)
        {
            return new AddGoalProgressResponse
            {
                Success = false,
                Message = "Hedef bulunamadı veya size ait değil.",
                Data = 0
            };
        }

        if (goal.TargetDate.Date < DateTime.UtcNow.Date)
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("TargetDate", "Bu hedefin süresi dolmuş, ilerleme kaydedilemez.")
            };
            throw new ValidationException(failures);
        }

        var today = DateTime.UtcNow.Date;

        var previousTotal = goal.TrackingRecords.Sum(r => r.ProgressValue);
        var newTotal = previousTotal + request.Value;
        goal.CurrentValue = newTotal;

        var activeTracking = goal.TrackingRecords.FirstOrDefault(r => r.RecordDate.Date == today);

        if (activeTracking != null)
        {
            activeTracking.ProgressValue += request.Value;
            activeTracking.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            activeTracking = new GoalTracking
            {
                GoalId = goal.Id,
                UserId = request.UserId,
                ProgressValue = request.Value,
                RecordDate = today,
                InsertedAt = DateTime.UtcNow,
                IsCompletedForInterval = false
            };
            _context.GoalTrackings.Add(activeTracking);
        }

        var wasCompleted = goal.TargetValue.HasValue && previousTotal >= goal.TargetValue.Value;
        var isCompletedNow = goal.TargetValue.HasValue && newTotal >= goal.TargetValue.Value;

        if (isCompletedNow)
        {
            goal.IsCompleted = true;
            activeTracking.IsCompletedForInterval = true;

            var allGoalRecords = await _context.GoalTrackings
                .Where(t => t.GoalId == goal.Id)
                .ToListAsync(cancellationToken);

            foreach (var record in allGoalRecords)
            {
                record.IsCompletedForInterval = true;
            }
        }

        if (!wasCompleted && isCompletedNow)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            if (userProfile != null)
            {
                userProfile.TotalScore += 50;
            }
            else
            {
                var newProfile = new UserProfile { UserId = request.UserId, TotalScore = 50 };
                _context.UserProfiles.Add(newProfile);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new AddGoalProgressResponse
        {
            Success = true,
            Message = "İlerleme başarıyla kaydedildi.",
            Data = activeTracking.Id
        };
    }
}
