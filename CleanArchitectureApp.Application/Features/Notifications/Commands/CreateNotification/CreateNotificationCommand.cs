using MediatR;

namespace CleanArchitectureApp.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommand : IRequest<CreateNotificationResponse>
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
