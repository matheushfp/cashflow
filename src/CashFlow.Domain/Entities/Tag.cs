namespace CashFlow.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public Enums.Tag Value { get; set; }
    
    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}