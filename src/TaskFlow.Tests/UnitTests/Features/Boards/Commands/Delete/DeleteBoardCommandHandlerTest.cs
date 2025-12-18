using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Commands.Delete;

public class DeleteBoardCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board);

        var request = DeleteBoardCommandBuilder.Build(board);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(user, board, id: board.Id);

        var request = DeleteBoardCommandBuilder.Build(board);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    private static DeleteBoardCommandHandler CreateHandler(Domain.Entities.User user, Board board, Guid? id = null)
    {
        var userRetriever = UserRetrieverBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        var repository = new BoardWriteOnlyRepositoryBuilder();
        if (id.HasValue)
        {
            repository.GetById(user, board, id);
        }
        else
        {
            repository.GetById(user, board);
        }

        return new DeleteBoardCommandHandler(repository.Build(), userRetriever, unitOfWork);
    }
}