using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.RefreshToken;

namespace TaskFlow.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IUserRetriever userRetriever,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteOnlyRepository repository) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userRetriever.GetCurrentUser();
        
        await repository.Delete(user.Id);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}