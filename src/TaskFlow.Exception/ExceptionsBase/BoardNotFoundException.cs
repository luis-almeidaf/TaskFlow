using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class BoardNotFoundException : TaskFlowException
{

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public BoardNotFoundException() : base(ResourceErrorMessages.BOARD_NOT_FOUND) { }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}