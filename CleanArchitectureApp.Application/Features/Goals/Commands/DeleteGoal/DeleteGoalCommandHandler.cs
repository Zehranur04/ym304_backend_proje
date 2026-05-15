using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.DeleteGoal;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand, DeleteGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public DeleteGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteGoalResponse> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (goal == null)
        {
            return new DeleteGoalResponse
            {
                Success = false,
                Message = "Goal not found."
            };
        }

        // Soft Delete
        goal.IsDeleted = true;
        goal.UpdatedAt = DateTime.UtcNow;

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == goal.UserId, cancellationToken);
        if (userProfile != null)
        {
            userProfile.TotalScore -= 50;
            if (userProfile.TotalScore < 0) userProfile.TotalScore = 0;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteGoalResponse
        {
            Success = true,
            Message = "Goal deleted successfully."
        };
    }
}
