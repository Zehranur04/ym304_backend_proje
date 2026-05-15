using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.AddGoalProgress;

public class AddGoalProgressCommandValidator : AbstractValidator<AddGoalProgressCommand>
{
    public AddGoalProgressCommandValidator()
    {
        RuleFor(v => v.Value)
            .GreaterThan(0).WithMessage("İlerleme miktarı 0'dan büyük olmalıdır.");
    }
}
