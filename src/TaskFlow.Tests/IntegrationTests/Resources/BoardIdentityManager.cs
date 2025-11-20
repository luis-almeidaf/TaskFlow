using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.IntegrationTests.Resources;

public class BoardIdentityManager
{
    private readonly Board _board;

    public BoardIdentityManager(Board board)
    {
        _board = board;
    }

    public Guid GetId() => _board.Id;
}