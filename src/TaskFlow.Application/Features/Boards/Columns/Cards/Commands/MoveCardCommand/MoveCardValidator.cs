using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;

public class MoveCardValidator : AbstractValidator<MoveCardCommand>
{
    public MoveCardValidator()
    {
        RuleFor(column => column.NewPosition).GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.NEW_POSITION_CANNOT_BE_NEGATIVE);
    }
}