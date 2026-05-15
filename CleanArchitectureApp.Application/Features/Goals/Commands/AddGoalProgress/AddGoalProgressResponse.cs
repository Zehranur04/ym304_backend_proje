namespace CleanArchitectureApp.Application.Features.Goals.Commands.AddGoalProgress;

public class AddGoalProgressResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Data { get; set; }
}
