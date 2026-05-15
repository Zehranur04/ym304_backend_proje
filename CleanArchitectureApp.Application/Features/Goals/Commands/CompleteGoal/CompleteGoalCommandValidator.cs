using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CompleteGoal;

public class CompleteGoalCommandValidator : AbstractValidator<CompleteGoalCommand>
{
    public CompleteGoalCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
