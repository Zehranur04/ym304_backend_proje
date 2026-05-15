using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitById;

public class GetHabitByIdQueryHandler : IRequestHandler<GetHabitByIdQuery, GetHabitByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetHabitByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetHabitByIdResponse> Handle(GetHabitByIdQuery request, CancellationToken cancellationToken)
    {
        var habit = await _context.Habits
            .Include(h => h.Category)
            .Where(h => h.Id == request.Id)
            .Select(h => new HabitDetailDto
            {
                Id = h.Id,
                Title = h.Title,
                Description = h.Description,
                CategoryName = h.Category.Name,
                TargetValue = h.TargetValue,
                Unit = h.Unit,
                TrackingType = h.TrackingType,
                Frequency = h.Frequency,
                InsertedAt = h.InsertedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (habit == null)
        {
            return new GetHabitByIdResponse
            {
                Success = false,
                Message = "Alışkanlık bulunamadı",
                Data = null
            };
        }

        return new GetHabitByIdResponse
        {
            Success = true,
            Message = "Habit retrieved successfully.",
            Data = habit
        };
    }
}
