using MediatR;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Application.Features.Users.Commands.GetByEmail;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailCommand, GetUserByEmailResponse?>
{
    private readonly IUserReadOnlyRepository _repository;

    public GetUserByEmailHandler(IUserReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetUserByEmailResponse?> Handle(GetUserByEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByEmail(request.Email);

        if (user is null) return null;
        
        return new GetUserByEmailResponse
        {
            Email = user.Email,
            Name = user.Name
        };
    }
}