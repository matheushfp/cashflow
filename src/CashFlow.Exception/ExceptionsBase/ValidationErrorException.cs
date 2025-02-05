namespace CashFlow.Exception.ExceptionsBase;

public class ValidationErrorException : CashFlowException
{
    public List<string> Errors { get; set; }

    public ValidationErrorException(List<string> errorMessages) : base(string.Empty)
    {
        Errors = errorMessages;
    }
}
