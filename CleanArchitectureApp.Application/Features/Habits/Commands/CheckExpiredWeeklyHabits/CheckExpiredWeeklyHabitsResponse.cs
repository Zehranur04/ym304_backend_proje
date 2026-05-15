namespace CleanArchitectureApp.Application.Features.Habits.Commands.CheckExpiredWeeklyHabits;

public class CheckExpiredWeeklyHabitsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Data { get; set; }
}
