using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;

public class CreateColumnHandler : IRequestHandler<CreateColumnCommand, CreateColumnResponse>
{
    private readonly IBoardReadOnlyRepository _boardReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IColumnWriteOnlyRepository _repository;

    public CreateColumnHandler(
        IColumnWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser,
        IBoardReadOnlyRepository boardReadOnlyRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _boardReadOnlyRepository = boardReadOnlyRepository;
    }

    public async Task<CreateColumnResponse> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        
        var user = await _loggedUser.GetUserAndBoards();
        
        var board = await _boardReadOnlyRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = request.Adapt<Column>();
        column.Id = Guid.NewGuid();
        column.BoardId = board.Id;

        var columnsCount = board.Columns.Count;
        column.Position = columnsCount;

        await _repository.Add(column);

        await _unitOfWork.Commit();

        return new CreateColumnResponse
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            Name = column.Name,
            Position = column.Position
        };
    }
    
    private void Validate(CreateColumnCommand request)
    {
        var result = new CreateColumnValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}