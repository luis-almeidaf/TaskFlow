using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, CreateBoardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardWriteOnlyRepository _repository;

    public CreateBoardCommandHandler(IUnitOfWork unitOfWork, ILoggedUser loggedUser,
        IBoardWriteOnlyRepository repository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<CreateBoardResponse> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var board = request.Adapt<Board>();
        board.Id = Guid.NewGuid();
        board.CreatedById = loggedUser.Id;

        _repository.AddUser(board, loggedUser);

        var columns = new List<Column>
        {
            new() { Id = Guid.NewGuid(), Name = "Todo", Position = 0, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "In Progress", Position = 1, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "Done", Position = 2, BoardId = board.Id, Board = board },
        };

        foreach (var column in columns)
            board.Columns.Add(column);

        await _repository.Add(board);

        await _unitOfWork.Commit();

        return new CreateBoardResponse
        {
            Id = board.Id,
            Name = board.Name,
        };
    }

    private static void Validate(CreateBoardCommand request)
    {
        var result = new CreateBoardValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}