using FluentValidation;
using TaskFlow.Application.Common.Validators;
using TaskFlow.Communication.Requests;
using TaskFlow.Exception;

namespace TaskFlow.Application.Features.Users.Commands.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RegisterUserCommand>())
            .WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
    
}