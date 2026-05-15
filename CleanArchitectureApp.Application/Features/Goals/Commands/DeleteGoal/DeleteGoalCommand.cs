using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.DeleteGoal;

public class DeleteGoalCommand : IRequest<DeleteGoalResponse>
{
    public int Id { get; set; }
}
