using FluentAssertions;
using TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Users;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Users.AddUser;

public class AddUserCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board);

        var request = AddUserCommandBuilder.Build(board, user);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Error_Board_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board, id: board.Id);

        var request = AddUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_UserToAdd_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board, email: user.Email);

        var request = AddUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Error_UserToAdd_Already_In_Board()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);
        board.Users.Add(user);

        var handler = CreateHandler(user, board);

        var request = AddUserCommandBuilder.Build(board, user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserAlreadyInBoardException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_ALREADY_IN_BOARD));
    }

    private static AddUserCommandHandler CreateHandler(
        User user,
        Board board,
        Guid? id = null,
        string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var repository = new BoardWriteOnlyRepositoryBuilder();
        repository.GetById(user, board);

        if (id.HasValue)
            repository.GetById(user, board, id);

        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        userReadOnlyRepository.GetUserByEmail(user, email);

        if (string.IsNullOrWhiteSpace(email))
            userReadOnlyRepository.GetUserByEmail(user);

        return new AddUserCommandHandler(unitOfWork, loggedUser, repository.Build(), userReadOnlyRepository.Build());
    }
}