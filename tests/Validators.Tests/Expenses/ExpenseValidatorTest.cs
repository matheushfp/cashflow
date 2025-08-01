﻿using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses;

public class ExpenseValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void EmptyTitleError(string title)
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();
        request.Title = title;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void InvalidAmountError(decimal amount)
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_0));
    }

    [Fact]
    public void FutureDateError()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(4);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSE_CANNOT_BE_IN_THE_FUTURE));
    }

    [Fact]
    public void InvalidTagError()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();
        request.Tags.Add((Tag)444);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_TAG));
    }

    [Fact]
    public void InvalidPaymentTypeError()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = ExpenseRequestBuilder.Build();
        request.PaymentType = (PaymentType)10;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }
}
