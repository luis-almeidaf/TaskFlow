using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Users;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Users.Commands.Delete;

public class DeleteUserCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var handler = CreateHandler(user, board: null);

        var request = DeleteUserCommandBuilder.Build();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_User_With_Boards()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        user.CreatedBoards.Add(board);

        var handler = CreateHandler(user, board);

        var request = DeleteUserCommandBuilder.Build();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserHasAssociatedBoardsException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_WITH_BOARD));
    }

    private static DeleteUserCommandHandler CreateHandler(User user, Board? board)
    {
        var userRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var boardRepository = new BoardReadOnlyRepositoryBuilder();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (board is not null)
        {
            boardRepository.GetAll(user, board);
        }
        else
        {
            boardRepository.GetAll(user);
        }

        return new DeleteUserCommandHandler(userRepository, boardRepository.Build(), loggedUser, unitOfWork);
    }
}