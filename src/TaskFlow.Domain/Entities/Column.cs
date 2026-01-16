namespace TaskFlow.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }

    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}