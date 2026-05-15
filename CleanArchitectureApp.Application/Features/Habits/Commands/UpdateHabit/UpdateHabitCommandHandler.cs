using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.UpdateHabit;

public class UpdateHabitCommandHandler : IRequestHandler<UpdateHabitCommand, UpdateHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateHabitResponse> Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (habit == null)
        {
            return new UpdateHabitResponse
            {
                Success = false,
                Message = "Habit not found."
            };
        }

        habit.Title = request.Title;
        habit.Description = request.Description;
        habit.TargetValue = request.TargetValue;
        habit.Unit = request.Unit;
        habit.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateHabitResponse
        {
            Id = habit.Id,
            Success = true,
            Message = "Habit updated successfully."
        };
    }
}
