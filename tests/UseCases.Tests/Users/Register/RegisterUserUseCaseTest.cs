using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
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

    [Fact]
    public async Task EmptyNameError()
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase();
        
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMPTY_NAME));
    }

    [Fact]
    public async Task EmailAlreadyExistsError()
    {
        var request = RegisterUserRequestBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        var usersReadOnlyRepository = new UsersReadOnlyRepositoryBuilder();
        var usersWriteOnlyRepository = UsersWriteOnlyRepositoryBuilder.Build();
        var tokenGenerator = AccessTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            usersReadOnlyRepository.UserWithThisEmailAlreadyExists(email);

        return new RegisterUserUseCase(mapper, passwordEncrypter, usersReadOnlyRepository.Build(), usersWriteOnlyRepository, tokenGenerator, unitOfWork);
    }
}
