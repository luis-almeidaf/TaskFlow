using FluentAssertions;
using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery;
using TaskFlow.Application.Features.Boards.Queries.GetBoardsQuery;
using TaskFlow.Domain.Entities;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Queries.GetAll;

public class GetBoardsQueryHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board);

        var request = new GetBoardsQuery();

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Boards.Should().HaveCount(1);
        result.Boards[0].Id.Should().Be(board.Id);
        result.Boards[0].Name.Should().Be(board.Name);
    }
    
    [Fact]
    public async Task Return_EmptyList_When_UserHasNoBoard()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board: null);

        var request = new GetBoardsQuery();

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Boards.Should().BeEmpty();
    }

    private static GetBoardsQueryHandler CreateHandler(Domain.Entities.User user, Board? board)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new BoardReadOnlyRepositoryBuilder();

        if (board is not null)
        {
            repository.GetAll(user, board);
        }
        else
        {
            repository.GetAll(user);
        }

        return new GetBoardsQueryHandler(loggedUser, repository.Build());
    }
}