using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class CardNotInColumnException() : TaskFlowException(ResourceErrorMessages.CARD_NOT_IN_COLUMN)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}