using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;

public class CreateColumnCommandHandler(
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardRepository,
    IColumnWriteOnlyRepository columnRepository) : IRequestHandler<CreateColumnCommand, CreateColumnResponse>
{
    public async Task<CreateColumnResponse> Handle(CreateColumnCommand request,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.GetUserAndBoards();

        var board = await boardRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = request.Adapt<Column>();
        column.Id = Guid.NewGuid();
        column.BoardId = board.Id;

        var columnsCount = board.Columns.Count;
        column.Position = columnsCount;

        await columnRepository.Add(column);

        await unitOfWork.Commit();

        return new CreateColumnResponse
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            Name = column.Name,
            Position = column.Position
        };
    }

    private static void Validate(CreateColumnCommand request)
    {
        var result = new CreateColumnValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}