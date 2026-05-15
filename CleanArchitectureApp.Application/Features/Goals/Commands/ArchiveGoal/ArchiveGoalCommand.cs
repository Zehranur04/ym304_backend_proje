using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.ArchiveGoal;

public class ArchiveGoalCommand : IRequest<ArchiveGoalResponse>
{
    public int Id { get; set; }
}
