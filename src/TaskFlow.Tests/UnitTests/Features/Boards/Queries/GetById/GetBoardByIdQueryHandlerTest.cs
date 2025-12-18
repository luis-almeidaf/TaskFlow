using FluentAssertions;
using TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Queries.GetById;

public class GetBoardByIdQueryHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board);

        var request = new GetBoardByIdQuery { Id = board.Id };

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

        var request = new GetBoardByIdQuery { Id = board.Id };
        
        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    private static GetBoardByIdQueryHandler CreateHandler(User user, Board board, Guid? id = null)
    {
        var userRetriever = UserRetrieverBuilder.Build(user);
        var repository = new BoardReadOnlyRepositoryBuilder();

        if (id.HasValue)
        {
            repository.GetById(user, board, id);
        }
        else
        {
            repository.GetById(user, board);
        }

        return new GetBoardByIdQueryHandler(repository.Build(), userRetriever);
    }
}