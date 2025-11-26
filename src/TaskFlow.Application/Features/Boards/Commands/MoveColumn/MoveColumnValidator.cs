using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.MoveColumn;

public class MoveColumnValidator : AbstractValidator<MoveColumnCommand>
{
    public MoveColumnValidator()
    {
        RuleFor(column => column.NewPosition).GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.NEW_POSITION_CANNOT_BE_NEGATIVE);
    }
}