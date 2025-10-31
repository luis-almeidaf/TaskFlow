namespace TaskFlow.Application.Common.Responses;

public class BaseErrorResponse
{
    public List<string> ErrorMessages { get;}

    public BaseErrorResponse(string errorMessages)
    {
        ErrorMessages = [errorMessages];
    }

    public BaseErrorResponse(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}