using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using FluentAssertions;

namespace UseCases.Tests.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = ChangePasswordRequestBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task EmptyNewPasswordError()
    {
        var user = UserBuilder.Build();
        var request = ChangePasswordRequestBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(e => e.GetErrors().Count == 1 &&
                          e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Fact]
    public async Task CurrentPasswordMismatchError()
    {
        var user = UserBuilder.Build();
        var request = ChangePasswordRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        
        result.Where(e => e.GetErrors().Count == 1 &&
                          e.GetErrors().Contains(ResourceErrorMessages.CURRENT_PASSWORD_MISMATCH));
    }
    
    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var usersUpdateOnlyRepository = UsersUpdateOnlyRepositoryBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, usersUpdateOnlyRepository, unitOfWork, passwordEncrypter);
    }
}