using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using TaskFlow.Application.UseCases.User.Register;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserDtoBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserDtoBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            readRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(
            unitOfWork,
            passwordEncrypter,
            tokenGenerator,
            readRepository.Build(),
            writeRepository);
    }
}