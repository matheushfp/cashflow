using CashFlow.Application.UseCases.Users.Register;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using CommonTestUtilities.Security.Tokens;
using FluentAssertions;

namespace UseCases.Tests.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.Token.Should().StartWith("eyJ");
    }

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var usersReadOnlyRepository = new UsersReadOnlyRepositoryBuilder().Build();
        var usersWriteOnlyRepository = UsersWriteOnlyRepositoryBuilder.Build();
        var tokenGenerator = AccessTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RegisterUserUseCase(mapper, passwordEncrypter, usersReadOnlyRepository, usersWriteOnlyRepository, tokenGenerator, unitOfWork);
    }
}
