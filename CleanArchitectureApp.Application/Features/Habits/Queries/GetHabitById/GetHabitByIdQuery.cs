using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitById;

public class GetHabitByIdQuery : IRequest<GetHabitByIdResponse>
{
    public int Id { get; set; }
}
