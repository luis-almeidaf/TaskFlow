using FluentValidation.Results;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.ChangePasswordCommand;

public class ChangePasswordCommandHandler(
    ICurrentUser currentUser,
    IUserWriteOnlyRepository repository,
    IPasswordEncrypter passwordEncrypter,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var loggedUser1 = await currentUser.GetCurrentUser();

        Validate(request, loggedUser1);

        var user = await repository.GetById(loggedUser1.Id);
        user!.Password = passwordEncrypter.Encrypt(request.NewPassword);

        await unitOfWork.Commit();

        return Unit.Value;
    }

    private void Validate(ChangePasswordCommand request, User user)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMatch = passwordEncrypter.Verify(request.Password, user.Password);

        if (!passwordMatch)
        {
            result.Errors.Add(new ValidationFailure(string.Empty,
                ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        if (result.IsValid) return;
        var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}