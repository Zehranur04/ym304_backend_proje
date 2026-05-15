using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CheckExpiredWeeklyHabits;

public class CheckExpiredWeeklyHabitsCommand : IRequest<CheckExpiredWeeklyHabitsResponse>
{
    public int UserId { get; set; }
}
