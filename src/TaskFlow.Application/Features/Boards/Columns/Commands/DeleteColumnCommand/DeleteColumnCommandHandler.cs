using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;

public class DeleteColumnCommandHandler(
    IBoardReadOnlyRepository boardReadOnlyRepository,
    ILoggedUser user,
    IUnitOfWork unitOfWork,
    IBoardWriteOnlyRepository repository)
    : IRequestHandler<DeleteColumnCommand, Unit>
{
    public async Task<Unit> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
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