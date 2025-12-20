using FluentAssertions;
using TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.Token;

namespace TaskFlow.Tests.UnitTests.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        const string refreshToken = "refreshToken";
        
        var refreshTokenEntity = RefreshTokenBuilder.Build();

        var handler = CreateHandler(refreshToken, refreshTokenEntity);

        var request = new RefreshTokenLoginCommand { RefreshToken = refreshToken };

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeEquivalentTo(refreshToken);
    }
    
    [Fact]
    public async Task Error_Refresh_Token_Expired()
    {
        const string refreshToken = "refreshToken";

        var refreshTokenEntity = RefreshTokenBuilder.Build();
        refreshTokenEntity.ExpiresOnUtc = DateTime.UtcNow;
        
        var handler = CreateHandler(refreshToken, refreshTokenEntity);

        var request = new RefreshTokenLoginCommand { RefreshToken = refreshToken };

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<RefreshTokenExpiredException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.REFRESH_TOKEN_EXPIRED));
    }
    
    private static RefreshTokenLoginCommandHandler CreateHandler(string token, RefreshToken refreshToken)
    {
        var refreshTokenRepository = RefreshTokenReadRepositoryBuilder.Build(token, refreshToken);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var tokenGenerator = IAccessTokenGeneratorBuilder.Build();

        return new RefreshTokenLoginCommandHandler(refreshTokenRepository, unitOfWork, tokenGenerator);
    }
}