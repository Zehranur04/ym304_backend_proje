using System;
using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalCommandValidator : AbstractValidator<CreateGoalCommand>
{
    public CreateGoalCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title cannot be empty.");

        RuleFor(v => v.TargetValue)
            .GreaterThan(0).When(v => v.TargetValue.HasValue)
            .WithMessage("TargetValue must be greater than 0.");

        RuleFor(v => v.TargetDate)
            .GreaterThan(DateTime.UtcNow.AddDays(-1)).WithMessage("Hedef tarihi geçmiş bir tarih olamaz.");
    }
}
