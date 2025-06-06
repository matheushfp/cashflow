﻿using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Register;
public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = ExpenseRequestBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task EmptyTitleError()
    {
        var user = UserBuilder.Build();
        var request = ExpenseRequestBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var mapper = MapperBuilder.Build();
        var expensesWriteOnlyRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(expensesWriteOnlyRepository, unitOfWork, mapper, loggedUser);
    }
}
