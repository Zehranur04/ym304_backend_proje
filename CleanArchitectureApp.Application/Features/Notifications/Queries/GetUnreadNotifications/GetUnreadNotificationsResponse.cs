using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Notifications.Queries.GetUnreadNotifications;

public class GetUnreadNotificationsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<NotificationDto> Data { get; set; } = new();
}
