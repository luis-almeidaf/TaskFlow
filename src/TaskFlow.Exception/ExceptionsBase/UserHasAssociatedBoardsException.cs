using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class UserHasAssociatedBoardsException : TaskFlowException
{
    public UserHasAssociatedBoardsException() : base(ResourceErrorMessages.USER_WITH_BOARD) { }
    public override int StatusCode => (int)HttpStatusCode.Conflict;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}