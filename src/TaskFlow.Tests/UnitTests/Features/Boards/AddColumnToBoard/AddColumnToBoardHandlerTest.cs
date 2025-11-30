using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;
using TaskFlow.Tests.CommonTestUtilities.LoggedUser;
using TaskFlow.Tests.CommonTestUtilities.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.AddColumnToBoard;

public class AddColumnToBoardHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user,board);

        var request = AddColumnToBoardCommandBuilder.Build(board, column);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.BoardId.Should().Be(board.Id);
        result.ColumnId.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(column.Name);
        result.Position.Should().Be(board.Columns.Count);
    }
    
    [Fact]
    public async Task Error_Board_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);
        
        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, boardId: board.Id);

        var request = AddColumnToBoardCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    private static CreateColumnCommandHandler CreateHandler(User user, Board board, Guid? boardId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.BuildUserWithBoards(user);
        var boardWriteRepository = new BoardWriteOnlyRepositoryBuilder();
        var boardReadRepository = new BoardReadOnlyRepositoryBuilder();
        
        boardReadRepository.GetById(user, board);

        if (boardId.HasValue)
            boardReadRepository.GetById(user, board, boardId);
        
        return new CreateColumnCommandHandler(boardWriteRepository.Build(), unitOfWork, loggedUser, boardReadRepository.Build());
    }
}