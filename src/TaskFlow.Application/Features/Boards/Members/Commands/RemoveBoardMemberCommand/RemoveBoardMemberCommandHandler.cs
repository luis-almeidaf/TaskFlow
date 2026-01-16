using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Members.Commands.RemoveBoardMemberCommand;

public class RemoveBoardMemberCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRetriever userRetriever,
    IBoardWriteOnlyRepository boardRepository) : IRequestHandler<RemoveBoardMemberCommand, Unit>
{
    public async Task<Unit> Handle(RemoveBoardMemberCommand request, CancellationToken cancellationToken)
    {
        await userRetriever.GetCurrentUser();

        var boardMemberToRemove = await boardRepository.GetBoardMember(request.BoardId, request.BoardMemberUserId)
                                  ?? throw new UserNotInBoardException();

        var ownerId = await boardRepository.GetOwnerId(request.BoardId);
        if (request.BoardMemberUserId == ownerId) throw new BoardOwnerCannotBeRemovedException();

        boardRepository.RemoveBoardMember(boardMemberToRemove);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}