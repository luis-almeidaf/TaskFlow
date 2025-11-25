using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteColumnFromBoard;

public class DeleteColumnFromBoardHandler(
    IBoardReadOnlyRepository boardReadOnlyRepository,
    ILoggedUser user,
    IUnitOfWork unitOfWork,
    IBoardWriteOnlyRepository repository)
    : IRequestHandler<DeleteColumnFromBoardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteColumnFromBoardCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await user.Get();

        var board = await repository.GetById(loggedUser, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await boardReadOnlyRepository.GetColumnById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        var deletedPosition = column.Position;

        repository.DeleteColumnFromBoard(column);
        
        repository.ReorderColumns(board, deletedPosition);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}