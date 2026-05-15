using MediatR;
using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.UpdateHabit;

public class UpdateHabitCommand : IRequest<UpdateHabitResponse>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? TargetValue { get; set; }
    public UnitType? Unit { get; set; }
}
