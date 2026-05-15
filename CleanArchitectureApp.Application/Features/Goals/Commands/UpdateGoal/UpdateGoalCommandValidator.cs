using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandValidator : AbstractValidator<UpdateGoalCommand>
{
    public UpdateGoalCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title cannot be empty.");
    }
}
