namespace CashFlow.Communication.Requests;

public class ChangePasswordRequest
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}