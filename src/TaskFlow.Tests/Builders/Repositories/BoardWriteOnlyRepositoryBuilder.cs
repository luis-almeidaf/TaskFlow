using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Tests.Builders.Repositories;

public class BoardWriteOnlyRepositoryBuilder
{
    private readonly Mock<IBoardWriteOnlyRepository> _repository = new();

    public BoardWriteOnlyRepositoryBuilder GetById(Board board, Guid? id = null)
    {
        if (id.HasValue)
        {
            _repository.Setup(repo => repo.GetById(id.Value)).ReturnsAsync((Board?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetById(board.Id)).ReturnsAsync(board);
        }

        return this;
    }

    public BoardWriteOnlyRepositoryBuilder GetBoardMember(BoardMember boardMember, Board board,
        Guid? fakeBoardMemberUserId = null)
    {
        if (fakeBoardMemberUserId.HasValue)
        {
            _repository.Setup(repo => repo.GetBoardMember(board.Id, It.IsAny<Guid>()))
                .ReturnsAsync((BoardMember?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetBoardMember(board.Id, boardMember.UserId)).ReturnsAsync(boardMember);
        }

        return this;
    }

    public BoardWriteOnlyRepositoryBuilder GetOwnerId(Board board, Guid ownerId)
    {
        _repository.Setup(repo => repo.GetOwnerId(board.Id)).ReturnsAsync(ownerId);

        return this;
    }

    public IBoardWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}