using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetCompletedHabits;

public class GetCompletedHabitsQueryHandler : IRequestHandler<GetCompletedHabitsQuery, GetCompletedHabitsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetCompletedHabitsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCompletedHabitsResponse> Handle(GetCompletedHabitsQuery request, CancellationToken cancellationToken)
    {
        var habits = await _context.Habits
            .Include(h => h.Category)
            .Where(h => h.UserId == request.UserId && h.IsCompleted == true && h.IsDeleted == false && h.IsArchived == false)
            .Select(h => new HabitListDto
            {
                Id = h.Id,
                Title = h.Title,
                CategoryName = h.Category.Name,
                TargetValue = h.TargetValue,
                Unit = h.Unit,
                TrackingType = h.TrackingType,
                Frequency = h.Frequency
            })
            .ToListAsync(cancellationToken);

        return new GetCompletedHabitsResponse
        {
            Success = true,
            Message = "Completed habits retrieved successfully.",
            Data = habits
        };
    }
}
