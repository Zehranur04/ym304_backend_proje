using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.UnarchiveGoal;

public class UnarchiveGoalCommandHandler : IRequestHandler<UnarchiveGoalCommand, UnarchiveGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public UnarchiveGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UnarchiveGoalResponse> Handle(UnarchiveGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (goal == null)
            return new UnarchiveGoalResponse { Success = false, Message = "Hedef bulunamadı." };

        goal.IsArchived = false;
        goal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UnarchiveGoalResponse { Success = true, Message = "Hedef arşivden çıkarıldı." };
    }
}
