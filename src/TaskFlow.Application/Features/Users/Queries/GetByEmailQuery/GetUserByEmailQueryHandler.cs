using MediatR;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Application.Features.Users.Queries.GetByEmailQuery;

public class GetUserByEmailQueryHandler(IUserReadOnlyRepository repository) : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse?>
{
    public async Task<GetUserByEmailResponse?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmail(request.Email);

        if (user is null) return null;

        return new GetUserByEmailResponse
        {
            Email = user.Email,
            Name = user.Name
        };
    }
}