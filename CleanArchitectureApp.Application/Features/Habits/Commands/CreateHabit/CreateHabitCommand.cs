using System.Text.Json.Serialization;
using CleanArchitectureApp.Domain.Enums;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CreateHabit;

public class CreateHabitCommand : IRequest<CreateHabitResponse>
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TrackingType TrackingType { get; set; }
    public FrequencyType Frequency { get; set; }
    public decimal? TargetValue { get; set; }
    public UnitType? Unit { get; set; }
}
