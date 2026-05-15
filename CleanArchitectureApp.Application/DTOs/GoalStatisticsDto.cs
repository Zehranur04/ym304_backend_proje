namespace CleanArchitectureApp.Application.DTOs;

public class GoalStatisticsDto
{
    public int CompletedGoalsCount { get; set; }
    public int ActiveGoalsCount { get; set; }
    public double SuccessRate { get; set; }
}
