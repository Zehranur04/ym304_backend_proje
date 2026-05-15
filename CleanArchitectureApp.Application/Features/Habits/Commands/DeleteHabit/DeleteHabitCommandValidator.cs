using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.DeleteHabit;

public class DeleteHabitCommandValidator : AbstractValidator<DeleteHabitCommand>
{
    public DeleteHabitCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
