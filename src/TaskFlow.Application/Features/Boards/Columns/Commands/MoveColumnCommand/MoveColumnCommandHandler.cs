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
    public async Task<Unit> Handle(MoveColumnCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        await userRetriever.GetCurrentUser();

        var board = await boardRepository.GetById(request.BoardId) ?? throw new BoardNotFoundException();

        var columns = board.Columns.OrderBy(column => column.Position).ToList();

        var columnToMove = columns.FirstOrDefault(column => column.Id == request.ColumnId) ?? throw new ColumnNotFoundException();

        columns.Remove(columnToMove);

        var targetPosition = Math.Min(request.NewPosition, columns.Count);
        columns.Insert(targetPosition, columnToMove);

        for (var i = 0; i < columns.Count; i++)
            columns[i].Position = i;

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