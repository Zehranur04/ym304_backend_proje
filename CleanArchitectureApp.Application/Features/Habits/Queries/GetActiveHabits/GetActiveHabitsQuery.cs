using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetActiveHabits;

public class GetActiveHabitsQuery : IRequest<GetActiveHabitsResponse>
{
    public int UserId { get; set; }
}
