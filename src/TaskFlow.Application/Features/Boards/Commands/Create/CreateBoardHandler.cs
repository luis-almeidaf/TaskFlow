using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.Create;

public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, CreateBoardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardWriteOnlyRepository _writeOnlyRepository;

    public CreateBoardHandler(IUnitOfWork unitOfWork, ILoggedUser loggedUser, IBoardWriteOnlyRepository writeOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _writeOnlyRepository = writeOnlyRepository;
    }

    public async Task<CreateBoardResponse> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        
        var loggedUser = await _loggedUser.Get();
        var board = request.Adapt<Board>();
        board.Id = Guid.NewGuid();
        board.CreatedById = loggedUser.Id;
        board.CreatedBy = loggedUser;

        await _writeOnlyRepository.Add(board);

        await _unitOfWork.Commit();

        return new CreateBoardResponse
        {
            Id = board.Id,
            Name = board.Name,
        };
    }

    private void Validate(CreateBoardCommand request)
    {
        var result = new CreateBoardValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}