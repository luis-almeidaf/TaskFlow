using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.RemoveBoardMemberCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Users;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Members.Commands.RemoveBoardMember;

public class RemoveBoardMemberCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var boardMemberToBeRemoved = BoardMemberBuilder.Build(user, board);

        var handler = CreateHandler(user, board, boardMemberToBeRemoved);

        var request = RemoveUserCommandBuilder.Build(board, boardMemberToBeRemoved);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_BoardMemberToRemove_Not_In_Board()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var boardMember = BoardMemberBuilder.Build(user, board);

        var nonExistingBoardMemberId = Guid.NewGuid();

        var handler = CreateHandler(user, board, boardMember, boardMemberId: nonExistingBoardMemberId);

        var request = new RemoveBoardMemberCommand { BoardId = board.Id, BoardMemberId = nonExistingBoardMemberId };

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotInBoardException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_IN_BOARD));
    }

    [Fact]
    public async Task Error_BoardOwner_Cannot_Be_Removed()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var boardMemberToBeRemoved = BoardMemberBuilder.Build(user, board);

        var handler = CreateHandler(user, board, boardMemberToBeRemoved, ownerId: boardMemberToBeRemoved.Id);

        var request = RemoveUserCommandBuilder.Build(board, boardMemberToBeRemoved);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardOwnerCannotBeRemovedException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_OWNER_CANNOT_BE_REMOVED));
    }

    private static RemoveBoardMemberCommandHandler CreateHandler(
        User user,
        Board board,
        BoardMember boardMember,
        Guid? ownerId = null,
        Guid? boardMemberId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRetriever = UserRetrieverBuilder.Build(user);
        var repository = new BoardWriteOnlyRepositoryBuilder();

        repository.GetBoardMember(boardMember, board);

        if (boardMemberId.HasValue)
            repository.GetBoardMember(boardMember, board, boardMemberId.Value);
        
        if (ownerId.HasValue)
            repository.GetOwnerId(board, ownerId.Value);
        
        return new RemoveBoardMemberCommandHandler(
            unitOfWork,
            userRetriever,
            repository.Build());
    }
}