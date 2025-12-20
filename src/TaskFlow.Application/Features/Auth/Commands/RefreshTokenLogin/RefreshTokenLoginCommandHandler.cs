using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandler(
    IRefreshTokenReadOnlyRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IAccessTokenGenerator tokenGenerator) : IRequestHandler<RefreshTokenLoginCommand, RefreshTokenLoginResponse>
{
    public async Task<RefreshTokenLoginResponse> Handle(RefreshTokenLoginCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetToken(request.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            throw new RefreshTokenExpiredException();

        var accessToken = tokenGenerator.Generate(refreshToken.User);

        refreshToken.Token = tokenGenerator.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await unitOfWork.Commit();

        return new RefreshTokenLoginResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken.Token
        };
    }
}