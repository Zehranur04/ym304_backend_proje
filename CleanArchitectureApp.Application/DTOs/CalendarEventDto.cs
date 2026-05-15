namespace CleanArchitectureApp.Application.DTOs;

public class CalendarEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Habit veya Goal
    public string Status { get; set; } = string.Empty;
    public decimal ProgressValue { get; set; }
    public decimal TargetValue { get; set; }
}
