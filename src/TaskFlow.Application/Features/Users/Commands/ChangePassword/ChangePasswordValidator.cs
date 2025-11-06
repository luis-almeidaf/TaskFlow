using FluentValidation;
using TaskFlow.Application.Common.Validators;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<ChangePasswordCommand>())
            .WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}