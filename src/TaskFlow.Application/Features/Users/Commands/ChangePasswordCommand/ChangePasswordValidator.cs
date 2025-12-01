using FluentValidation;
using TaskFlow.Application.Common.Validators;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Users.Commands.ChangePasswordCommand;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<Commands.ChangePasswordCommand.ChangePasswordCommand>())
            .WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}