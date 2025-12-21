using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserReadOnlyRepository userReadOnlyRepository,
    IRefreshTokenWriteOnlyRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userReadOnlyRepository.GetUserByEmail(command.Email) ?? throw new InvalidLoginException();
        var passwordMatch = passwordEncrypter.Verify(command.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenGenerator.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await refreshTokenRepository.Delete(user.Id);

        await refreshTokenRepository.Add(refreshToken);

        await unitOfWork.Commit();

        return new LoginResponse
        {
            Id = user.Id,
            Name = user.Name,
            Token = tokenGenerator.Generate(user),
            RefreshToken = refreshToken.Token,
        };
    }
}