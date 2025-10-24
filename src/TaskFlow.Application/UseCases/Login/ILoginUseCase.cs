using TaskFlow.Communication.Requests;
using TaskFlow.Communication.Responses;

namespace TaskFlow.Application.UseCases.Login;

public interface ILoginUseCase
{
    Task<ResponseRegisteredUserDto> Execute(RequestLoginDto request);
}