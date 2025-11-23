using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;

public class AddColumnToBoardValidator : AbstractValidator<AddColumnToBoardCommand>
{
    public AddColumnToBoardValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}