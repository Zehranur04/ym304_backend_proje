using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.ArchiveGoal;

public class ArchiveGoalCommandHandler : IRequestHandler<ArchiveGoalCommand, ArchiveGoalResponse>
{
    private readonly IApplicationDbContext _context;

    public ArchiveGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArchiveGoalResponse> Handle(ArchiveGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (goal == null)
            return new ArchiveGoalResponse { Success = false, Message = "Hedef bulunamadı." };

        goal.IsArchived = true;
        goal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ArchiveGoalResponse { Success = true, Message = "Hedef arşivlendi." };
    }
}
