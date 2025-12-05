using FluentAssertions;
using TaskFlow.Application.Features.Boards.Users.Commands.RemoveUserCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Users;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Users.Commands.RemoveUser;

public class RemoveUserCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var userToBeRemoved = UserBuilder.Build();
        board.Users.Add(userToBeRemoved);

        var handler = CreateHandler(user, board, userToBeRemoved);

        var request = RemoveUserCommandBuilder.Build(board, userToBeRemoved);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var userToBeRemoved = UserBuilder.Build();
        board.Users.Add(userToBeRemoved);

        var handler = CreateHandler(user, board, userToBeRemoved, board.Id);

        var request = RemoveUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_UserToRemove_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var userToBeRemoved = UserBuilder.Build();
        board.Users.Add(userToBeRemoved);

        var handler = CreateHandler(user, board, userToBeRemoved, userId: user.Id);

        var request = RemoveUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Error_UserToRemove_Not_In_Board()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var userToBeRemoved = UserBuilder.Build();

        var handler = CreateHandler(user, board, userToBeRemoved);

        var request = RemoveUserCommandBuilder.Build(board, user);

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
        board.Users.Add(user);
        
        var handler = CreateHandler(user, board);

        var request = RemoveUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardOwnerCannotBeRemovedException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_OWNER_CANNOT_BE_REMOVED));
    }

    private static RemoveUserCommandHandler CreateHandler(
        User user,
        Board board,
        User? userToBeRemoved = null,
        Guid? boardId = null,
        Guid? userId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var repository = new BoardWriteOnlyRepositoryBuilder();
        repository.GetById(user, board);

        if (boardId.HasValue)
            repository.GetById(user, board, boardId);

        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        userReadOnlyRepository.GetById(user);
        
        if (userToBeRemoved is not null)
            userReadOnlyRepository.GetById(userToBeRemoved);

        if (userId.HasValue)
            userReadOnlyRepository.GetById(user, userId);

        return new RemoveUserCommandHandler(
            unitOfWork,
            loggedUser,
            repository.Build(),
            userReadOnlyRepository.Build());
    }
}