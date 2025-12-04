using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class CardNotFoundException() : TaskFlowException(ResourceErrorMessages.CARD_NOT_FOUND)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}