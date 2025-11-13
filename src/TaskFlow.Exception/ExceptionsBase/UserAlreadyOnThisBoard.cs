using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class UserAlreadyInBoardException : TaskFlowException
{
    public UserAlreadyInBoardException() : base(ResourceErrorMessages.USER_ALREADY_IN_BOARD) { }
    
    public override int StatusCode => (int)HttpStatusCode.Conflict;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}