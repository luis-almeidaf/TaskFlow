using FluentValidation;
using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.UpdateColumn;

public class UpdateColumnValidator : AbstractValidator<UpdateColumnCommand>
{
    public UpdateColumnValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}