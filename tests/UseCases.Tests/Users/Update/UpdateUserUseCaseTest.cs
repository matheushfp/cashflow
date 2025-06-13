using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Users.Update;

public class UpdateUserUseCaseTest
{ 
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = UpdateUserRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task EmptyNameError()
    {
        var user = UserBuilder.Build();
        
        var request = UpdateUserRequestBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMPTY_NAME));
    }

    [Fact]
    public async Task EmailAlreadyExistsError()
    {
        var user = UserBuilder.Build();
        var request = UpdateUserRequestBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var usersUpdateOnlyRepository = UsersUpdateOnlyRepositoryBuilder.Build(user);
        var usersReadOnlyRepository = new UsersReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            usersReadOnlyRepository.UserWithThisEmailAlreadyExists(email);

        return new UpdateUserUseCase(loggedUser, usersReadOnlyRepository.Build(), usersUpdateOnlyRepository, unitOfWork);
    }
}