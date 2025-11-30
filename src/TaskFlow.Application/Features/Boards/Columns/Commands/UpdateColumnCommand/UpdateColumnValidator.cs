using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;

public class UpdateColumnValidator : AbstractValidator<UpdateColumnCommand>
{
    public UpdateColumnValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}