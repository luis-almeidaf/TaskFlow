using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.Register;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.CommonTestUtilities.Commands;
using TaskFlow.Tests.CommonTestUtilities.Cryptography;
using TaskFlow.Tests.CommonTestUtilities.Repositories;
using TaskFlow.Tests.CommonTestUtilities.Token;

namespace TaskFlow.Tests.UnitTests.Features.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RegisterUserCommandBuilder.Build();
        var handler = CreateHandler();


        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RegisterUserCommandBuilder.Build();
        request.Name = string.Empty;

        var handler = CreateHandler();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RegisterUserCommandBuilder.Build();

        var handler = CreateHandler(request.Email);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private static RegisterUserHandler CreateHandler(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            readRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserHandler(
            unitOfWork,
            passwordEncrypter,
            tokenGenerator,
            readRepository.Build(),
            writeRepository);
    }
}