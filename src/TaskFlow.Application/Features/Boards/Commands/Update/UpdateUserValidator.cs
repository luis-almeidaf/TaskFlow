using FluentValidation;
using TaskFlow.Application.Features.Boards.Commands.Create;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.Update;

public class UpdateUserValidator : AbstractValidator<UpdateBoardCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}