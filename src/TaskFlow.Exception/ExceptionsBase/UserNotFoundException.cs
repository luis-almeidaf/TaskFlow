using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class UserNotFoundException : TaskFlowException
{
    public UserNotFoundException() : base(ResourceErrorMessages.USER_NOT_FOUND) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}