using System.Net;

namespace TaskFlow.Exception.ExceptionsBase;

public class RefreshTokenExpiredException() : TaskFlowException(ResourceErrorMessages.REFRESH_TOKEN_EXPIRED)
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    
    public override List<string> GetErrors()
    {
        return [Message];
    }
}