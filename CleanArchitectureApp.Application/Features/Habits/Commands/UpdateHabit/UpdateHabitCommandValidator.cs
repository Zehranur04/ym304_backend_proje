using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.UpdateHabit;

public class UpdateHabitCommandValidator : AbstractValidator<UpdateHabitCommand>
{
    public UpdateHabitCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title cannot be empty.");
    }
}
