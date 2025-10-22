namespace TaskFlow.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}