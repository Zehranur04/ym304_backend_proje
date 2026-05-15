using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, CreateGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateGoalResponse> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Description = request.Description,
            TrackingType = request.TrackingType,
            Frequency = request.Frequency,
            TargetValue = request.TargetValue,
            Unit = request.Unit,
            TargetDate = request.TargetDate,
            CurrentValue = 0,
            IsCompleted = false,
            InsertedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Goals.Add(goal);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateGoalResponse
        {
            Id = goal.Id,
            Success = true,
            Message = "Goal created successfully."
        };
    }
}
