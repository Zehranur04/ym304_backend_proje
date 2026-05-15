using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.DeleteGoal;

public class DeleteGoalCommandValidator : AbstractValidator<DeleteGoalCommand>
{
    public DeleteGoalCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
