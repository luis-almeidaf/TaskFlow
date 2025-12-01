using MediatR;

namespace TaskFlow.Application.Features.Users.Queries.GetByEmailQuery;

public class GetUserByEmailQuery : IRequest<GetUserByEmailResponse?>
{
    public string Email { get; set; } = string.Empty; 
}