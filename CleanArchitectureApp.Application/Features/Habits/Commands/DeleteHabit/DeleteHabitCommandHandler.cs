using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.DeleteHabit;

public class DeleteHabitCommandHandler : IRequestHandler<DeleteHabitCommand, DeleteHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public DeleteHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteHabitResponse> Handle(DeleteHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (habit == null)
        {
            return new DeleteHabitResponse
            {
                Success = false,
                Message = "Habit not found."
            };
        }

        // Soft Delete
        habit.IsDeleted = true;
        habit.UpdatedAt = DateTime.UtcNow;

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == habit.UserId, cancellationToken);
        if (userProfile != null)
        {
            userProfile.TotalScore -= 10;
            if (userProfile.TotalScore < 0) userProfile.TotalScore = 0;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteHabitResponse
        {
            Success = true,
            Message = "Habit deleted successfully."
        };
    }
}
