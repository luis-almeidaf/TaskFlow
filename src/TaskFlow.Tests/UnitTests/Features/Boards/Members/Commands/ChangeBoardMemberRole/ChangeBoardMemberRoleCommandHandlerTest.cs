using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.ChangeBoardMemberRoleCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Members.Commands.ChangeBoardMemberRole;

public class ChangeBoardMemberRoleCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var boardMember = BoardMemberBuilder.Build(user, board);
        
        var handler = CreateHandler(board, boardMember);

        var request = new ChangeBoardMemberRoleCommand
        {
            BoardId = board.Id,
            BoardMemberUserId = boardMember.UserId,
            Role = BoardRole.Admin,
        };
        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_User_Not_In_Board()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var boardMember = BoardMemberBuilder.Build(user, board);
        
        var nonExistingBoardMemberId = Guid.NewGuid();
        var handler = CreateHandler(board, boardMember, nonExistingBoardMemberId: nonExistingBoardMemberId);

        var request = new ChangeBoardMemberRoleCommand
        {
            BoardId = board.Id,
            BoardMemberUserId = nonExistingBoardMemberId,
            Role = BoardRole.Admin,
        };
        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotInBoardException>();
        
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_IN_BOARD));
    }
    
    private static ChangeBoardMemberRoleCommandHandler CreateHandler(Board board, BoardMember boardMember,
        Guid? nonExistingBoardMemberId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var boardRepo = new BoardWriteOnlyRepositoryBuilder();

        boardRepo.GetBoardMember(boardMember, board);

        if (nonExistingBoardMemberId.HasValue)
            boardRepo.GetBoardMember(boardMember, board, nonExistingBoardMemberId.Value);

        return new ChangeBoardMemberRoleCommandHandler(unitOfWork, boardRepo.Build());
    }
}