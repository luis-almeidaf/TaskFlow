using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.GetByEmail;

public class GetUserByEmailCommand : IRequest<GetUserByEmailResponse?>
{
    public string Email { get; set; } = string.Empty; 
}