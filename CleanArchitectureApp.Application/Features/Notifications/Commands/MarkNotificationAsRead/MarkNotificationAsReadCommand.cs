using MediatR;

namespace CleanArchitectureApp.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommand : IRequest<MarkNotificationAsReadResponse>
{
    public int Id { get; set; }
    public int UserId { get; set; }
}
