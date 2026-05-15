using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitTrackingHistory;

public class GetHabitTrackingHistoryQueryHandler : IRequestHandler<GetHabitTrackingHistoryQuery, GetHabitTrackingHistoryResponse>
{
    private readonly IApplicationDbContext _context;

    public GetHabitTrackingHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetHabitTrackingHistoryResponse> Handle(GetHabitTrackingHistoryQuery request, CancellationToken cancellationToken)
    {
        DateTime? utcStartDate = request.StartDate.HasValue 
            ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc) 
            : null;

        DateTime? utcEndDate = request.EndDate.HasValue 
            ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) 
            : null;

        var query = _context.HabitTrackings
            .Where(t => t.HabitId == request.HabitId && t.UserId == request.UserId);

        if (utcStartDate.HasValue)
        {
            query = query.Where(t => t.RecordDate >= utcStartDate.Value.Date);
        }

        if (utcEndDate.HasValue)
        {
            query = query.Where(t => t.RecordDate <= utcEndDate.Value.Date);
        }

        var trackings = await query
            .OrderByDescending(t => t.RecordDate)
            .Select(t => new HabitTrackingDto
            {
                Id = t.Id,
                HabitId = t.HabitId,
                RecordDate = t.RecordDate,
                ProgressValue = t.ProgressValue,
                IsCompletedForDay = t.IsCompletedForDay
            })
            .ToListAsync(cancellationToken);

        return new GetHabitTrackingHistoryResponse
        {
            Success = true,
            Message = "Alışkanlık geçmişi başarıyla getirildi.",
            Data = trackings
        };
    }
}
