using FluentValidation;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;

public class AddBoardMemberValidator : AbstractValidator<AddBoardMemberCommand>
{
    public AddBoardMemberValidator()
    {
        RuleFor(request => request.UserEmail)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.UserEmail), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}