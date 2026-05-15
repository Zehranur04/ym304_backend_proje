using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetCompletedHabits;

public class GetCompletedHabitsQuery : IRequest<GetCompletedHabitsResponse>
{
    public int UserId { get; set; }
}
