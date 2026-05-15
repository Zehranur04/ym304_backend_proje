using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetArchivedHabits;

public class GetArchivedHabitsQuery : IRequest<GetArchivedHabitsResponse>
{
    public int UserId { get; set; }
}
