using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Security.Tokens;

namespace TaskFlow.Tests.Builders.Token;

public static class IAccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>())).Returns("token");
        mock.Setup(accessTokenGenerator => accessTokenGenerator.GenerateRefreshToken()).Returns("newRefreshToken");

        return mock.Object;
    }
}