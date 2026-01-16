using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardValidator() => RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
}