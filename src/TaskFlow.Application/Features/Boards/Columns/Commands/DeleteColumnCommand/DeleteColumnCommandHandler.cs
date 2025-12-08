using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;

public class DeleteColumnCommandHandler(
    ILoggedUser user,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardRepository,
    IColumnReadOnlyRepository columnReadOnlyRepository,
    IColumnWriteOnlyRepository columnRepository) : IRequestHandler<DeleteColumnCommand, Unit>
{
    public async Task<Unit> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await user.Get();

        var board = await boardRepository.GetById(loggedUser, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await columnReadOnlyRepository.GetById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        var deletedPosition = column.Position;

        columnRepository.Delete(column);

        columnRepository.ReorderColumns(board, deletedPosition);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}