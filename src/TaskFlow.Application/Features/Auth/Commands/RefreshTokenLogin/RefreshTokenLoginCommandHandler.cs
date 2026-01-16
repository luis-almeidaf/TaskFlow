using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandler(
    IRefreshTokenReadOnlyRepository refreshTokenRepository,
    IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    IAccessTokenGenerator tokenGenerator) : IRequestHandler<RefreshTokenLoginCommand, RefreshTokenLoginResponse>
{
    public async Task<RefreshTokenLoginResponse> Handle(RefreshTokenLoginCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetToken(request.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = refreshToken.UserId,
            Token = tokenGenerator.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        var accessToken = tokenGenerator.Generate(refreshToken.User);

        await refreshTokenWriteOnlyRepository.Delete(refreshToken.UserId);

        await refreshTokenWriteOnlyRepository.Add(newRefreshToken);

        await unitOfWork.Commit();

        return new RefreshTokenLoginResponse
        {
            Token = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}