using FluentValidation;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.AddHabitProgress;

public class AddHabitProgressCommandValidator : AbstractValidator<AddHabitProgressCommand>
{
    public AddHabitProgressCommandValidator()
    {
        RuleFor(v => v.Value)
            .GreaterThan(0).WithMessage("İlerleme miktarı 0'dan büyük olmalıdır.");
    }
}
