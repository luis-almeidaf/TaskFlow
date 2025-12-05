using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Tests.Builders.Repositories;

public class BoardReadOnlyRepositoryBuilder
{
    private readonly Mock<IBoardReadOnlyRepository> _repository = new();

    public BoardReadOnlyRepositoryBuilder GetById(User user, Board board, Guid? id = null)
    {
        if (id.HasValue)
        {
            _repository.Setup(repo => repo.GetById(It.IsAny<User>(), id.Value)).ReturnsAsync((Board?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetById(It.IsAny<User>(), board.Id)).ReturnsAsync(board);
        }

        return this;
    }

    public BoardReadOnlyRepositoryBuilder GetAll(User user, Board? board = null)
    {
        if (board is not null)
        {
            _repository.Setup(repo => repo.GetAll(user)).ReturnsAsync([board]);
        }
        else
        {
            _repository.Setup(repo => repo.GetAll(user)).ReturnsAsync([]);
        }

        return this;
    }

    public BoardReadOnlyRepositoryBuilder GetColumnById(Column column, Guid? id = null)
    {
        if (id.HasValue)
        {
            _repository.Setup(repo => repo.GetColumnById(id.Value)).ReturnsAsync((Column?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetColumnById(column.Id)).ReturnsAsync(column);
        }

        return this;
    }

    public IBoardReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}