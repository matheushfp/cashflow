using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = ExpenseRequestBuilder.Build();

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        await act.Should().NotThrowAsync();
        
        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((CashFlow.Domain.Enums.PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task EmptyTitleError()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = ExpenseRequestBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task ExpenseNotFoundError()
    {
        var loggedUser = UserBuilder.Build();
        var request = ExpenseRequestBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var expensesUpdateOnlyRepository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        
        return new UpdateExpenseUseCase(mapper, unitOfWork, expensesUpdateOnlyRepository, loggedUser);
    }
    
}