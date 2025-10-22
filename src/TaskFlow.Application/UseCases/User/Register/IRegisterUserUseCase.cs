using TaskFlow.Communication.Requests;
using TaskFlow.Communication.Responses;

namespace TaskFlow.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserDto> Execute(RequestRegisterUserDto request);
}