namespace CleanArchitectureApp.Application.Features.Goals.Commands.CheckExpiredGoals;

public class CheckExpiredGoalsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Data { get; set; } // Expired goal count
}
