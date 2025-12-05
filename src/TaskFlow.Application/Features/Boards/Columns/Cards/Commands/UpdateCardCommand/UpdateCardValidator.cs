using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;

public class UpdateCardValidator : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardValidator()
    {
        RuleFor(card => card.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_CANNOT_BE_EMPTY);
        RuleFor(card => card.DueDate).GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.CARD_DUE_DATE_CANNOT_BE_FOR_THE_PAST);
    }
}