using FluentAssertions;
using TaskFlow.Application.Features.Auth.Commands.Login;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Login;
using TaskFlow.Tests.Builders.Cryptography;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.Token;

namespace TaskFlow.Tests.UnitTests.Features.Auth.Commands.Login;

public class LoginCommandHandlerTest
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
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
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


    private static LoginCommandHandler CreateHandler(User user, string? password = null)
    {
        var userRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
        var refreshTokenRepository = RefreshTokenWriteRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var tokenGenerator = IAccessTokenGeneratorBuilder.Build();
        
        return new LoginCommandHandler(userRepository, refreshTokenRepository, unitOfWork, passwordEncrypter, tokenGenerator);
    }
}