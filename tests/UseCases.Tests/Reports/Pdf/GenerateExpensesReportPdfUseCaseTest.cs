using CashFlow.Application.UseCases.Reports.Pdf;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Tests.Reports.Pdf;

public class GenerateExpensesReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);
        
        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task EmptySuccess()
    {
        var loggedUser = UserBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser, []);
        
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
        
        result.Should().BeEmpty();
    }
    
    private GenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        
        return new GenerateExpensesReportPdfUseCase(repository, loggedUser);
    }
}