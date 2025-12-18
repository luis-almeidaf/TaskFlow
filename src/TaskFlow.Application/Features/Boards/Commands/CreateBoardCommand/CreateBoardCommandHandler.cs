using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRetriever userRetriever,
    IBoardWriteOnlyRepository repository) : IRequestHandler<CreateBoardCommand, CreateBoardResponse>
{
    public async Task<CreateBoardResponse> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await userRetriever.GetCurrentUser();

        var board = request.Adapt<Board>();
        board.Id = Guid.NewGuid();
        board.CreatedById = user.Id;

        var boardMember = new BoardMember
        {
            Id = Guid.NewGuid(),
            BoardId = board.Id,
            Role = BoardRole.Owner,
            UserId = user.Id
        };

        repository.AddBoardMember(boardMember);

        var columns = new List<Column>
        {
            new() { Id = Guid.NewGuid(), Name = "Todo", Position = 0, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "In Progress", Position = 1, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "Done", Position = 2, BoardId = board.Id, Board = board },
        };

        foreach (var column in columns)
            board.Columns.Add(column);

        await repository.Add(board);

        await unitOfWork.Commit();

        return new CreateBoardResponse
        {
            Id = board.Id,
            Name = board.Name,
        };
    }

    private static void Validate(CreateBoardCommand request)
    {
        var result = new CreateBoardValidator().Validate(request);

        if (result.IsValid) return;
        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}