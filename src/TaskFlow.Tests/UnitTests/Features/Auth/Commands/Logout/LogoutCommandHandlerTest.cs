using FluentAssertions;
using TaskFlow.Application.Features.Auth.Commands.Logout;
using TaskFlow.Domain.Entities;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Auth.Commands.Logout;

public class LogoutCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var handler = CreateHandler(user);

        var request = new LogoutCommand();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
    
    private static LogoutCommandHandler CreateHandler(User user)
    {
        var userRetriever = UserRetrieverBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var refreshTokenRepository = RefreshTokenWriteRepositoryBuilder.Build();

        return new LogoutCommandHandler(userRetriever, unitOfWork ,refreshTokenRepository);
    }
}