using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;

public class DeleteColumnCommandHandler(
    IUserRetriever userRetriever,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardRepository,
    IColumnReadOnlyRepository columnReadOnlyRepository,
    IColumnWriteOnlyRepository columnRepository) : IRequestHandler<DeleteColumnCommand, Unit>
{
    public async Task<Unit> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        await userRetriever.GetCurrentUser();

        var board = await boardRepository.GetById(request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await columnReadOnlyRepository.GetById(board.Id, request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        var deletedPosition = column.Position;

        columnRepository.Delete(column);

        columnRepository.ReorderColumns(board, deletedPosition);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}