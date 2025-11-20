using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.Delete;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.CommonTestUtilities.Commands;
using TaskFlow.Tests.CommonTestUtilities.Entities;
using TaskFlow.Tests.CommonTestUtilities.LoggedUser;
using TaskFlow.Tests.CommonTestUtilities.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Users.Delete;

public class DeleteUserHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var handler = CreateHandler(user);

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

        var handler = CreateHandler(user);

        var request = DeleteUserCommandBuilder.Build();
        
        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserHasAssociatedBoardsException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_WITH_BOARD));
    }

    private static DeleteUserHandler CreateHandler(Domain.Entities.User user)
    {
        var repository = UserWriteOnlyRepositoryBuilder.Build();
        var loggedUser = LoggedUserBuilder.BuildUserWithBoards(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserHandler(repository, loggedUser, unitOfWork);
    }
}