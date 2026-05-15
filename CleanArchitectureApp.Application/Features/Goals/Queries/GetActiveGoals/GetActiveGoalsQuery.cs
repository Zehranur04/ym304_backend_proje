using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetActiveGoals;

public class GetActiveGoalsQuery : IRequest<GetActiveGoalsResponse>
{
    public int UserId { get; set; }
}
