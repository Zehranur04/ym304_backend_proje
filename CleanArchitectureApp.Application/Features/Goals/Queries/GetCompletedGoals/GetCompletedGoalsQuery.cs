using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetCompletedGoals;

public class GetCompletedGoalsQuery : IRequest<GetCompletedGoalsResponse>
{
    public int UserId { get; set; }
}
