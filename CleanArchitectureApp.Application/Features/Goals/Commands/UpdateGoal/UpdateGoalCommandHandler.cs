using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand, UpdateGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateGoalResponse> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (goal == null)
        {
            return new UpdateGoalResponse
            {
                Success = false,
                Message = "Goal not found."
            };
        }

        goal.Title = request.Title;
        goal.Description = request.Description;
        goal.TargetValue = request.TargetValue;
        goal.Unit = request.Unit;
        goal.TargetDate = request.TargetDate;
        goal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateGoalResponse
        {
            Id = goal.Id,
            Success = true,
            Message = "Goal updated successfully."
        };
    }
}
