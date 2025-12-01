using MediatR;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Application.Features.Users.Queries.GetByEmailQuery;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse?>
{
    private readonly IUserReadOnlyRepository _repository;

    public GetUserByEmailQueryHandler(IUserReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetUserByEmailResponse?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
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