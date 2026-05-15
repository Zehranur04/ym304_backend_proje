using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.UnarchiveHabit;

public class UnarchiveHabitCommandHandler : IRequestHandler<UnarchiveHabitCommand, UnarchiveHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public UnarchiveHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UnarchiveHabitResponse> Handle(UnarchiveHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (habit == null)
            return new UnarchiveHabitResponse { Success = false, Message = "Alışkanlık bulunamadı." };

        habit.IsArchived = false;
        habit.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UnarchiveHabitResponse { Success = true, Message = "Alışkanlık arşivden çıkarıldı." };
    }
}
