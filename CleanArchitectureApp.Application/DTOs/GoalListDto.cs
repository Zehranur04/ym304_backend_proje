using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Application.DTOs;

public class GoalListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal? TargetValue { get; set; }
    public decimal? CurrentValue { get; set; }
    public UnitType? Unit { get; set; }
    public FrequencyType Frequency { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsArchived { get; set; }
}
