namespace TaskFlow.Exception.ExceptionsBase;

public abstract class TaskFlowException : SystemException
{
    protected TaskFlowException(string message) : base(message) { }
    
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}