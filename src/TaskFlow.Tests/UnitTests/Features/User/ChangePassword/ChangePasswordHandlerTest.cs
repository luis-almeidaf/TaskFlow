using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.ChangePassword;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.CommonTestUtilities.Commands;
using TaskFlow.Tests.CommonTestUtilities.Cryptography;
using TaskFlow.Tests.CommonTestUtilities.Entities;
using TaskFlow.Tests.CommonTestUtilities.LoggedUser;
using TaskFlow.Tests.CommonTestUtilities.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.User.ChangePassword;

public class ChangePasswordHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = ChangePasswordCommandBuilder.Build();

        var handler = CreateHandler(user, request.Password);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();

        var request = ChangePasswordCommandBuilder.Build();
        request.NewPassword = string.Empty;

        var handler = CreateHandler(user, request.Password);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result =await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }
    
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();

        var request = ChangePasswordCommandBuilder.Build();

        var handler = CreateHandler(user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result =await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }

    private static ChangePasswordHandler CreateHandler(Domain.Entities.User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();

        return new ChangePasswordHandler(loggedUser, updateRepository, passwordEncrypter, unitOfWork);
    }
}