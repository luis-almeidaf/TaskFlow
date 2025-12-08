namespace TaskFlow.Domain.Repositories.Column;

public interface IColumnWriteOnlyRepository
{
    Task Add(Entities.Column column);
    void Delete(Entities.Column column);
    void ReorderColumns(Entities.Board board, int position);
    void Update(Entities.Column column);
    void UpdateRange(IEnumerable<Entities.Column> columns);
}