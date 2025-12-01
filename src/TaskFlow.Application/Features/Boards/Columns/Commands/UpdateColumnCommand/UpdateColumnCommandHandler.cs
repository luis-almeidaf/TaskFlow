using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;

public class UpdateColumnCommandHandler(
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser,
    IBoardReadOnlyRepository readOnlyRepository,
    IBoardWriteOnlyRepository repository)
    : IRequestHandler<UpdateColumnCommand, Unit>
{
    public async Task<Unit> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.Get();

        var board = await repository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await readOnlyRepository.GetColumnById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        column.Name = request.Name;

        repository.UpdateColumn(column);

        await unitOfWork.Commit();

        return Unit.Value;
    }

    private static void Validate(UpdateColumnCommand request)
    {
        var result = new UpdateColumnValidator().Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}