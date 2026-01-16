using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommandHandler(
    IBoardWriteOnlyRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBoardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await repository.GetById(request.Id) ?? throw new BoardNotFoundException();

        await repository.Delete(request.Id);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}