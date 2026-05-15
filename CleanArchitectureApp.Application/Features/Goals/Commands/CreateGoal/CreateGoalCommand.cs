using System.Text.Json.Serialization;
using CleanArchitectureApp.Domain.Enums;
using CleanArchitectureApp.Domain.Enums;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalCommand : IRequest<CreateGoalResponse>
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
    public DateTime TargetDate { get; set; }
}
