namespace CleanArchitectureApp.Application.DTOs;

public class HabitStatisticsDto
{
    public int CompletedHabitsCount { get; set; }
    public int ActiveHabitsCount { get; set; }
    public double SuccessRate { get; set; }
}
