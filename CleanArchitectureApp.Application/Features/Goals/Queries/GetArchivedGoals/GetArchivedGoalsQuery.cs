using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetArchivedGoals;

public class GetArchivedGoalsQuery : IRequest<GetArchivedGoalsResponse>
{
    public int UserId { get; set; }
}
