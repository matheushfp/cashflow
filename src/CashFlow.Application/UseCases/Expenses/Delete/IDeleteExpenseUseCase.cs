namespace CashFlow.Application.UseCases.Expenses.Delete;
public interface IDeleteExpenseUseCase
{
    Task Execute(Guid id);
}
