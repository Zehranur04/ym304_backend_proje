using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CompleteGoal;

public class CompleteGoalCommand : IRequest<CompleteGoalResponse>
{
    public int Id { get; set; }
}
