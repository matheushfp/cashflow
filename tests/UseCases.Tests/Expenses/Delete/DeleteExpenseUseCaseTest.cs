using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Delete;
public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ExpenseNotFoundError()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: Guid.NewGuid());

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var readOnlyRepository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var writeOnlyRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(readOnlyRepository, writeOnlyRepository, unitOfWork, loggedUser);
    }
}
