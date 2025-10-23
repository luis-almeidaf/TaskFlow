using FluentValidation.Results;
using Mapster;
using TaskFlow.Communication.Requests;
using TaskFlow.Communication.Responses;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;

    public RegisterUserUseCase(
        IUnitOfWork unitOfWork, 
        IPasswordEncrypter passwordEncrypter, 
        IAccessTokenGenerator accessTokenGenerator,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
        _tokenGenerator = accessTokenGenerator;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
    }

    public async Task<ResponseRegisteredUserDto> Execute(RequestRegisterUserDto request)
    {
        await Validate(request);
        
        var user = request.Adapt<Domain.Entities.User>();
        user.Password = _passwordEncrypter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisteredUserDto
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserDto request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        
        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}