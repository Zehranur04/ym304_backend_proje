using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Notifications.Queries.GetUnreadNotifications;

public class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, GetUnreadNotificationsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUnreadNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUnreadNotificationsResponse> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetUnreadNotificationsResponse
        {
            Success = true,
            Message = "Okunmamış bildirimler başarıyla getirildi.",
            Data = notifications
        };
    }
}
