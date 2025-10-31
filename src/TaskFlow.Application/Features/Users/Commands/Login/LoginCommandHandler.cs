using MediatR;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public LoginCommandHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncrypter passwordEncrypter, 
        IAccessTokenGenerator tokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();
        var passwordMatch = _passwordEncrypter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        return new LoginResponse
        {
            Id = user.Id,
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };

    }
}
