using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;

public class MoveColumnCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRetriever userRetriever,
    IBoardWriteOnlyRepository boardRepository,
    IColumnWriteOnlyRepository columnRepository) : IRequestHandler<MoveColumnCommand, Unit>
{
    /// <summary>
    /// Handles the movement of a column to a new position within a board.
    /// </summary>
    /// <param name="request">The command containing the board ID, column ID, and the new position.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Unit"/> value indicating successful completion.</returns>
    /// <exception cref="BoardNotFoundException">Thrown when the specified board is not found or does not belong to the logged userRetriever.</exception>
    /// <exception cref="ColumnNotFoundException">Thrown when the specified column is not found within the board.</exception>
    /// <remarks>
    /// This method reorders all columns in the board after the move operation to ensure sequential positioning.
    /// If the requested position exceeds the number of columns, the column is moved to the last position.
    /// </remarks>
    public async Task<Unit> Handle(MoveColumnCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        await userRetriever.GetCurrentUser();

        var board = await boardRepository.GetById(request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var columns = board.Columns.OrderBy(column => column.Position).ToList();

        var columnToMove = columns.FirstOrDefault(column => column.Id == request.ColumnId);
        if (columnToMove is null) throw new ColumnNotFoundException();

        columns.Remove(columnToMove);

        var targetPosition = Math.Min(request.NewPosition, columns.Count);
        columns.Insert(targetPosition, columnToMove);

        for (var i = 0; i < columns.Count; i++)
        {
            columns[i].Position = i;
        }

        columnRepository.UpdateRange(columns);

        await unitOfWork.Commit();
        return Unit.Value;
    }

    private static void Validate(MoveColumnCommand request)
    {
        var result = new MoveColumnValidator().Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}