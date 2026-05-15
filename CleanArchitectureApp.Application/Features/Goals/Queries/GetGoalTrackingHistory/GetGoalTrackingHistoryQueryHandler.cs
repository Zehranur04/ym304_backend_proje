using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalTrackingHistory;

public class GetGoalTrackingHistoryQueryHandler : IRequestHandler<GetGoalTrackingHistoryQuery, GetGoalTrackingHistoryResponse>
{
    private readonly IApplicationDbContext _context;

    public GetGoalTrackingHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetGoalTrackingHistoryResponse> Handle(GetGoalTrackingHistoryQuery request, CancellationToken cancellationToken)
    {
        DateTime? utcStartDate = request.StartDate.HasValue 
            ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc) 
            : null;

        DateTime? utcEndDate = request.EndDate.HasValue 
            ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) 
            : null;

        var query = _context.GoalTrackings
            .Where(t => t.GoalId == request.GoalId && t.UserId == request.UserId);

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
            .Select(t => new GoalTrackingDto
            {
                Id = t.Id,
                GoalId = t.GoalId,
                RecordDate = t.RecordDate,
                ProgressValue = t.ProgressValue
            })
            .ToListAsync(cancellationToken);

        return new GetGoalTrackingHistoryResponse
        {
            Success = true,
            Message = "Hedef geçmişi başarıyla getirildi.",
            Data = trackings
        };
    }
}
