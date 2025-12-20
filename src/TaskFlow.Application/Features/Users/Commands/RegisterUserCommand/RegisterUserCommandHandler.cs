using FluentValidation.Results;
using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator,
    IRefreshTokenWriteOnlyRepository refreshTokenRepository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserWriteOnlyRepository userWriteOnlyRepository) : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await Validate(request);

        var user = request.Adapt<User>();
        user.Password = passwordEncrypter.Encrypt(request.Password);
        user.Id = Guid.NewGuid();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenGenerator.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await refreshTokenRepository.Add(refreshToken);
        await userWriteOnlyRepository.Add(user);

        await unitOfWork.Commit();

        return new RegisterUserResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Token = tokenGenerator.Generate(user),
            RefreshToken = refreshToken.Token
        };
    }

    private async Task Validate(RegisterUserCommand request)
    {
        var result = await new RegisterUserValidator().ValidateAsync(request);

        var emailExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}