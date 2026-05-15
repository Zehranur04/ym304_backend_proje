using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.UnarchiveGoal;

public class UnarchiveGoalCommand : IRequest<UnarchiveGoalResponse>
{
    public int Id { get; set; }
}
