using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.CreateHabit;

public class CreateHabitCommandValidator : AbstractValidator<CreateHabitCommand>
{
    public CreateHabitCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title cannot be empty.");

        RuleFor(v => v.TargetValue)
            .GreaterThan(0).When(v => v.TargetValue.HasValue)
            .WithMessage("TargetValue must be greater than 0.");
    }
}
