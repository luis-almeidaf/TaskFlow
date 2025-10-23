using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Security.Tokens;

namespace CommonTestUtilities.Token;

public static class JwtTokenGenerator
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>())).Returns("token");

        return mock.Object;
    }
}