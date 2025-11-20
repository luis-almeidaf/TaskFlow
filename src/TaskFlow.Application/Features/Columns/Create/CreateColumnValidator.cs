using FluentValidation;
using TaskFlow.Application.Features.Boards.Commands.Create;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Columns.Create;

public class CreateColumnValidator : AbstractValidator<CreateColumnCommand>
{
    public CreateColumnValidator()
    {
        RuleFor(board => board.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
    }
}