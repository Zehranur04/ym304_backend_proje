using System;
using CleanArchitectureApp.Domain.Enums;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommand : IRequest<UpdateGoalResponse>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? TargetValue { get; set; }
    public UnitType? Unit { get; set; }
    public DateTime TargetDate { get; set; }
}
