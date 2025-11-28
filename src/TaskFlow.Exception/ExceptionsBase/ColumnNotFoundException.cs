using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class ColumnNotFoundException : TaskFlowException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public ColumnNotFoundException() : base(ResourceErrorMessages.COLUMN_NOT_FOUND) { }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}