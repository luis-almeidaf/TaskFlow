using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Tests.CommonTestUtilities.Repositories;

public class BoardReadOnlyRepositoryBuilder
{
    private readonly Mock<IBoardReadOnlyRepository> _repository = new();

    public BoardReadOnlyRepositoryBuilder GetById(User user, Board board, Guid? id = null)
    {
        if (id.HasValue)
        {
            _repository.Setup(repo => repo.GetById(user, id.Value)).ReturnsAsync((Board?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetById(user, board.Id)).ReturnsAsync(board);
        }

        return this;
    }

    public IBoardReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}