using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.ArchiveHabit;

public class ArchiveHabitCommand : IRequest<ArchiveHabitResponse>
{
    public int Id { get; set; }
}
