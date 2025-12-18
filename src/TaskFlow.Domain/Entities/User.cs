namespace TaskFlow.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public ICollection<BoardMember> Boards { get; set; } = new HashSet<BoardMember>();
    
    public ICollection<Board> CreatedBoards { get; set; } = new HashSet<Board>();
    public ICollection<Card> CreatedCards { get; set; } = new HashSet<Card>();
    public ICollection<Card> AssignedCards { get; set; } = new HashSet<Card>();
}