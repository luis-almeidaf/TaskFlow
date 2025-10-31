using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.Login;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace UseCases.Test.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = LoginCommandBuilder.Build();
        request.Email = user.Email;

        var handler = CreateHandler(user, request.Password);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var request = LoginCommandBuilder.Build();

        var handler = CreateHandler(user, request.Password);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();

        var request = LoginCommandBuilder.Build();
        request.Email = user.Email;

        var handler = CreateHandler(user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }


    private static LoginCommandHandler CreateHandler(TaskFlow.Domain.Entities.User user, string? password = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new LoginCommandHandler(readRepository, passwordEncrypter, tokenGenerator);
    }
}