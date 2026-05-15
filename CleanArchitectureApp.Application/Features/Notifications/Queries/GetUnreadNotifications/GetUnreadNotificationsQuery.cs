using MediatR;

namespace CleanArchitectureApp.Application.Features.Notifications.Queries.GetUnreadNotifications;

public class GetUnreadNotificationsQuery : IRequest<GetUnreadNotificationsResponse>
{
    public int UserId { get; set; }
}
