using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Tests.Users.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute();
        
        await act.Should().NotThrowAsync();
    }

    private DeleteUserUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = UsersWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new DeleteUserUseCase(loggedUser, repository, unitOfWork);
    }
}