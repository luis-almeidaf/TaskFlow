using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Tests.Builders.Entities;

namespace TaskFlow.Tests.Builders.Repositories;

public class RefreshTokenReadRepositoryBuilder
{
    public static IRefreshTokenReadOnlyRepository Build(string token, RefreshToken refreshToken)
    {
        var mock = new Mock<IRefreshTokenReadOnlyRepository>();
        
        mock.Setup(repo => repo.GetToken(token)).ReturnsAsync(refreshToken);
        
        return mock.Object;
    }
}