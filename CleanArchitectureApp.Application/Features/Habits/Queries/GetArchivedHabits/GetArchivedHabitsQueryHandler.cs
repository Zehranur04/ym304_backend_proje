using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetArchivedHabits;

public class GetArchivedHabitsQueryHandler : IRequestHandler<GetArchivedHabitsQuery, GetArchivedHabitsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetArchivedHabitsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetArchivedHabitsResponse> Handle(GetArchivedHabitsQuery request, CancellationToken cancellationToken)
    {
        var habits = await _context.Habits
            .Include(h => h.Category)
            .Where(h => h.UserId == request.UserId && h.IsArchived)
            .Select(h => new HabitListDto
            {
                Id = h.Id,
                Title = h.Title,
                CategoryName = h.Category.Name,
                TargetValue = h.TargetValue,
                Unit = h.Unit,
                TrackingType = h.TrackingType,
                Frequency = h.Frequency,
                CurrentValue = 0,
                CompletedThisPeriod = false,
                DaysUntilAvailable = null
            })
            .ToListAsync(cancellationToken);

        return new GetArchivedHabitsResponse
        {
            Success = true,
            Message = "Archived habits retrieved successfully.",
            Data = habits
        };
    }
}
