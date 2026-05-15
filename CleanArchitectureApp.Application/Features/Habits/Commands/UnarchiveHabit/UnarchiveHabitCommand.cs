using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.UnarchiveHabit;

public class UnarchiveHabitCommand : IRequest<UnarchiveHabitResponse>
{
    public int Id { get; set; }
}
