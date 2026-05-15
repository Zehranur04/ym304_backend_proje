using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CompleteHabit;

public class CompleteHabitCommand : IRequest<CompleteHabitResponse>
{
    public int Id { get; set; }
}
