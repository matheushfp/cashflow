using CashFlow.Application.UseCases.Users.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using CommonTestUtilities.Security.Tokens;
using FluentAssertions;

namespace UseCases.Tests.Users.Login;
public class UserLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = LoginRequestBuilder.Build();
        request.Email = user.Email;
        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.Token.Should().StartWith("eyJ");
    }

    [Fact]
    public async Task UserNotFoundError()
    {
        var user = UserBuilder.Build();
        var request = LoginRequestBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidCredentialsException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_CREDENTIALS));
    }

    [Fact]
    public async Task PasswordNotMatchError()
    {
        var user = UserBuilder.Build();
        var request = LoginRequestBuilder.Build();
        request.Email = user.Email;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidCredentialsException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_CREDENTIALS));
    }

    private UserLoginUseCase CreateUseCase(User user, string? password = null)
    {
        var usersReadOnlyRepository = new UsersReadOnlyRepositoryBuilder().FindUserByEmail(user).Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();

        return new UserLoginUseCase(usersReadOnlyRepository, passwordEncrypter, accessTokenGenerator);
    }
}
