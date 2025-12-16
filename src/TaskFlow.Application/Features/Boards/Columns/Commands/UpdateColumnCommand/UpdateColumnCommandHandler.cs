using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;

public class UpdateColumnCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IBoardReadOnlyRepository boardRepository,
    IColumnReadOnlyRepository readOnlyRepository,
    IColumnWriteOnlyRepository columnRepository)
    : IRequestHandler<UpdateColumnCommand, Unit>
{
    public async Task<Unit> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await currentUser.GetCurrentUser();

        var board = await boardRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await readOnlyRepository.GetById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        column.Name = request.Name;

        columnRepository.Update(column);

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