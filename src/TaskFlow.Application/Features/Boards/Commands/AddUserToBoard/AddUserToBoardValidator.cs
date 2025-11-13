using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;

public class AddUserToBoardValidator : AbstractValidator<AddUserToBoardCommand>
{
    public AddUserToBoardValidator()
    {
        RuleFor(request => request.UserEmail)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.UserEmail), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}