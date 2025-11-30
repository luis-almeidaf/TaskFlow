using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;

public class AddUserValidator : AbstractValidator<AddUserCommand>
{
    public AddUserValidator()
    {
        RuleFor(request => request.UserEmail)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.UserEmail), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}