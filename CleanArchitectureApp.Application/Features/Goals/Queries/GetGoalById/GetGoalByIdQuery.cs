using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalById;

public class GetGoalByIdQuery : IRequest<GetGoalByIdResponse>
{
    public int Id { get; set; }
}
