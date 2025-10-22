namespace TaskFlow.Communication.Responses;

public class ResponseErrorDto
{
    public List<string> ErrorMessages { get;}

    public ResponseErrorDto(string errorMessages)
    {
        ErrorMessages = [errorMessages];
    }

    public ResponseErrorDto(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}