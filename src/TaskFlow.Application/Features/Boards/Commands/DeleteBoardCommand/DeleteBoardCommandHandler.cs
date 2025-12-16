using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommandHandler(
    IBoardWriteOnlyRepository repository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBoardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await currentUser.GetCurrentUser();

        var board = await repository.GetById(user,request.Id);
        if (board is null) throw new BoardNotFoundException();

        await repository.Delete(request.Id);

        await unitOfWork.Commit();
        
        return Unit.Value;
    }
}