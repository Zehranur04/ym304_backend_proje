using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, MarkNotificationAsReadResponse>
{
    private readonly IApplicationDbContext _context;

    public MarkNotificationAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MarkNotificationAsReadResponse> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == request.UserId, cancellationToken);

        if (notification == null)
        {
            return new MarkNotificationAsReadResponse
            {
                Success = false,
                Message = "Bildirim bulunamadı."
            };
        }

        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);

        return new MarkNotificationAsReadResponse
        {
            Success = true,
            Message = "Bildirim okundu olarak işaretlendi."
        };
    }
}
