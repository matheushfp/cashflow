namespace CashFlow.Communication.Responses;

public class ErrorResponse
{
    public List<string> Errors {  get; set; }

    public ErrorResponse(string message)
    {
        Errors = [message];
    }

    public ErrorResponse(List<string> errorMessages)
    {
        Errors = errorMessages;
    }
}
