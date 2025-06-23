using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();
        var request = ChangePasswordRequestBuilder.Build();
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void EmptyNewPasswordError(string newPassword)
    {
        var validator = new ChangePasswordValidator();
        var request = ChangePasswordRequestBuilder.Build();
        request.NewPassword = newPassword;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}