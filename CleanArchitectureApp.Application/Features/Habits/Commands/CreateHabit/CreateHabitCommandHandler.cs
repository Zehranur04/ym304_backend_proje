using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CreateHabit;

public class CreateHabitCommandHandler : IRequestHandler<CreateHabitCommand, CreateHabitResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateHabitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateHabitResponse> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = new Habit
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Description = request.Description,
            TrackingType = request.TrackingType,
            Frequency = request.Frequency,
            TargetValue = request.TargetValue,
            Unit = request.Unit,
            StartDate = DateTime.UtcNow,
            InsertedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Habits.Add(habit);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateHabitResponse
        {
            Id = habit.Id,
            Success = true,
            Message = "Habit created successfully."
        };
    }
}
