using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.GetByEmail;
using TaskFlow.Tests.CommonTestUtilities.Commands;
using TaskFlow.Tests.CommonTestUtilities.Entities;
using TaskFlow.Tests.CommonTestUtilities.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.User.GetByEmail;

public class GetUserByEmailHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = GetUserByEmailCommandBuilder.Build(user);

        var handler = CreateHandler(user);

        var result = await handler.Handle(request, CancellationToken.None);

        result!.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var request = GetUserByEmailCommandBuilder.Build(user);

        var handler = CreateHandler(user, email: user.Email);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();
    }

    private GetUserByEmailHandler CreateHandler(Domain.Entities.User user, string? email = null)
    {
        var repository = new UserReadOnlyRepositoryBuilder();
        if (string.IsNullOrWhiteSpace(email))
        {
            repository.GetUserByEmail(user);
        }
        else
        {
            repository.GetUserByEmail(user, email);
        }

        return new GetUserByEmailHandler(repository.Build());
    }
}