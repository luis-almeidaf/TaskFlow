using FluentValidation.Results;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateRepository _repository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(ILoggedUser loggedUser, IUserUpdateRepository repository,
        IPasswordEncrypter passwordEncrypter, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _passwordEncrypter = passwordEncrypter;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);
        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

        await _unitOfWork.Commit();

        return Unit.Value;
    }

    private void Validate(ChangePasswordCommand request, User loggedUser)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMatch = _passwordEncrypter.Verify(request.Password, loggedUser.Password);

        if (!passwordMatch)
        {
            result.Errors.Add(new ValidationFailure(string.Empty,
                ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}