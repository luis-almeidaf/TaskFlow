using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class BoardOwnerCannotBeRemovedException : TaskFlowException
{
    public BoardOwnerCannotBeRemovedException() : base(ResourceErrorMessages.BOARD_OWNER_CANNOT_BE_REMOVED) { }
    public override int StatusCode => (int)HttpStatusCode.Conflict;
    public override List<string> GetErrors() => [Message];
}