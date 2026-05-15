using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.DeleteHabit;

public class DeleteHabitCommand : IRequest<DeleteHabitResponse>
{
    public int Id { get; set; }
}
