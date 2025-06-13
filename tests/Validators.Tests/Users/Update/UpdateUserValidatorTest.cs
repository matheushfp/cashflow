using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var request = UpdateUserRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void EmptyNameError(string name)
    {
        var validator = new UpdateUserValidator();
        var request = UpdateUserRequestBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_NAME));
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void EmptyEmailError(string email)
    {
        var validator = new UpdateUserValidator();
        var request = UpdateUserRequestBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_EMAIL));
    }

    [Fact]
    public void InvalidEmailError()
    {
        var validator = new UpdateUserValidator();
        var request = UpdateUserRequestBuilder.Build();
        request.Email = "johndoe.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_EMAIL));
    }
}