using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Members.Commands.ChangeBoardMemberRoleCommand;

public class ChangeBoardMemberRoleCommandHandler(
    IUnitOfWork unitOfWork,
    IBoardWriteOnlyRepository repository) : IRequestHandler<ChangeBoardMemberRoleCommand, Unit>
{
    public async Task<Unit> Handle(ChangeBoardMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var boardMember = await repository.GetBoardMember(request.BoardId, request.BoardMemberUserId) 
                          ?? throw new UserNotInBoardException();

        boardMember.Role = request.Role;

        repository.UpdateBoardMember(boardMember);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}