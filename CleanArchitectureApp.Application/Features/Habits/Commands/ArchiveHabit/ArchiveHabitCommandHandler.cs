using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.ArchiveHabit;

public class ArchiveHabitCommandHandler : IRequestHandler<ArchiveHabitCommand, ArchiveHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public ArchiveHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArchiveHabitResponse> Handle(ArchiveHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (habit == null)
            return new ArchiveHabitResponse { Success = false, Message = "Alışkanlık bulunamadı." };

        habit.IsArchived = true;
        habit.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ArchiveHabitResponse { Success = true, Message = "Alışkanlık arşivlendi." };
    }
}
