using System;
using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Application.DTOs;

public class HabitDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal? TargetValue { get; set; }
    public UnitType? Unit { get; set; }
    public TrackingType TrackingType { get; set; }
    public FrequencyType Frequency { get; set; }
    public DateTime InsertedAt { get; set; }
}
