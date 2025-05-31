using CashFlow.Domain.Entities;

namespace WebAPI.Tests.Resources;
public class ExpenseIdentityManager
{
    private readonly Expense _expense;

    public ExpenseIdentityManager(Expense expense)
    {
        _expense = expense;
    }

    public Guid GetId() => _expense.Id;
    public DateTime GetDate() => _expense.Date;
}
