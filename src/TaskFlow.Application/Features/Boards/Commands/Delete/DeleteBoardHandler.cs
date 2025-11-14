using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.Delete;

public class DeleteBoardHandler(
    IBoardWriteOnlyRepository repository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBoardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await loggedUser.Get();

        var board = await repository.GetById(user,request.Id);
        if (board is null) throw new BoardNotFoundException();

        await repository.Delete(request.Id);

        await unitOfWork.Commit();
        
        return Unit.Value;
    }
}