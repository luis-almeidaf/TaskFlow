using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;

public class UpdateBoardValidator : AbstractValidator<UpdateBoardCommand>
{
    public UpdateBoardValidator() => RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
}
