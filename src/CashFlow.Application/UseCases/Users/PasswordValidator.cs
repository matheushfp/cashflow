using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.Users;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";

    public override bool IsValid(FluentValidation.ValidationContext<T> context, string password)
    {
        return false;
    }
}
