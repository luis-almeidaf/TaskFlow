using TaskFlow.Communication.Requests;
using TaskFlow.Communication.Responses;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.UseCases.Login;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _tokenGenerator;


    public LoginUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator tokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserDto> Execute(RequestLoginDto request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

        if (user == null)
            throw new InvalidLoginException();

        var passwordMatch = _passwordEncrypter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        return new ResponseRegisteredUserDto
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }
}