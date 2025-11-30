using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;

public class CreateColumnValidator : AbstractValidator<CreateColumnCommand>
{
    public CreateColumnValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}