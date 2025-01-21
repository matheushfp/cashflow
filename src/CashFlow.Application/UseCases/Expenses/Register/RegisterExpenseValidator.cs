using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RegisterExpenseRequest>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("Amount should be greater than 0");
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date of an expense cannot be in the future");
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Payment Type is not valid");
    }
}
