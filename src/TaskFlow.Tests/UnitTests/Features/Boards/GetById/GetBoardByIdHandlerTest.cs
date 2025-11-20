using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.GetById;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;
using TaskFlow.Tests.CommonTestUtilities.LoggedUser;
using TaskFlow.Tests.CommonTestUtilities.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.GetById;

public class GetBoardByIdHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board);

        var request = GetBoardByIdCommandBuilder.Build(board);

        var result = await handler.Handle(request, CancellationToken.None);

        result!.Id.Should().Be(request.Id);
        result.Name.Should().Be(board.Name);
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board, id: board.Id);

        var request = GetBoardByIdCommandBuilder.Build(board);

        
        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    private static GetBoardByIdHandler CreateHandler(Domain.Entities.User user, Board board, Guid? id = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new BoardReadOnlyRepositoryBuilder();

        if (id.HasValue)
        {
            repository.GetById(user, board, id);
        }
        else
        {
            repository.GetById(user, board);
        }

        return new GetBoardByIdHandler(repository.Build(), loggedUser);
    }
}