using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class UserNotInBoardException : TaskFlowException
{
    public UserNotInBoardException() : base(ResourceErrorMessages.USER_NOT_IN_BOARD) { }
    
    public override int StatusCode => (int)HttpStatusCode.NotFound;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}