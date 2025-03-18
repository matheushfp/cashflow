
using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class InvalidCredentialsException : CashFlowException
{
    public InvalidCredentialsException() : base(ResourceErrorMessages.INVALID_CREDENTIALS)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
