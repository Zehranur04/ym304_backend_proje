namespace CleanArchitectureApp.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
